using Microsoft.JSInterop;
using MuniLK.Web.Services;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MuniLK.Web.Services
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly TokenProvider _tokenProvider;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CustomAuthStateProvider _authStateProvider;
        private readonly ILogger<AuthTokenHandler> _logger;

        public AuthTokenHandler(
            TokenProvider tokenProvider, 
            IHttpClientFactory httpClientFactory,
            CustomAuthStateProvider authStateProvider,
            ILogger<AuthTokenHandler> logger)
        {
            _tokenProvider = tokenProvider;
            _httpClientFactory = httpClientFactory;
            _authStateProvider = authStateProvider;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _tokenProvider.GetToken();
            _logger.LogDebug("AuthTokenHandler called with token: {TokenExists}", !string.IsNullOrEmpty(token));
            
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            // If 401 Unauthorized, try to refresh token
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && !string.IsNullOrEmpty(token))
            {
                _logger.LogInformation("Received 401, attempting token refresh");
                
                var refreshSucceeded = await TryRefreshTokenAsync();
                
                if (refreshSucceeded)
                {
                    _logger.LogInformation("Token refresh succeeded, retrying request");
                    
                    // Clone the original request (we need to re-read the content)
                    var clonedRequest = await CloneRequestAsync(request);
                    
                    // Set the new token
                    var newToken = _tokenProvider.GetToken();
                    if (!string.IsNullOrEmpty(newToken))
                    {
                        clonedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
                    }

                    // Retry the request
                    response.Dispose(); // Dispose the old response
                    response = await base.SendAsync(clonedRequest, cancellationToken);
                }
                else
                {
                    _logger.LogWarning("Token refresh failed, redirecting to login");
                    
                    // Clear tokens and notify auth state change
                    _tokenProvider.ClearToken();
                    await _authStateProvider.MarkUserAsUnauthenticated();
                }
            }

            return response;
        }

        private async Task<bool> TryRefreshTokenAsync()
        {
            try
            {
                // Create a fresh HTTP client without the AuthTokenHandler to avoid recursion
                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("http://localhost:5164/"); // API base URL

                var response = await httpClient.PostAsync("api/auth/refresh", null);
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var refreshResponse = JsonSerializer.Deserialize<AuthRefreshResponse>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    
                    if (refreshResponse?.Succeeded == true && !string.IsNullOrEmpty(refreshResponse.AccessToken))
                    {
                        _tokenProvider.SetToken(refreshResponse.AccessToken);
                        await _authStateProvider.MarkUserAsAuthenticated(refreshResponse.AccessToken);
                        return true;
                    }
                }
                
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return false;
            }
        }

        private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request)
        {
            var clonedRequest = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Version = request.Version
            };

            // Copy headers
            foreach (var header in request.Headers)
            {
                clonedRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            // Copy content if present
            if (request.Content != null)
            {
                var contentBytes = await request.Content.ReadAsByteArrayAsync();
                clonedRequest.Content = new ByteArrayContent(contentBytes);

                // Copy content headers
                foreach (var header in request.Content.Headers)
                {
                    clonedRequest.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            return clonedRequest;
        }
    }

    public class AuthRefreshResponse
    {
        public bool Succeeded { get; set; }
        public string? AccessToken { get; set; }
        public string? Message { get; set; }
    }
}
