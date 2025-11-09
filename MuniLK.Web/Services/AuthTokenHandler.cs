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
        private readonly IJSRuntime _jsRuntime;

        public AuthTokenHandler(
            TokenProvider tokenProvider,
            IHttpClientFactory httpClientFactory,
            CustomAuthStateProvider authStateProvider,
            ILogger<AuthTokenHandler> logger,
            IJSRuntime jsRuntime)
        {
            _tokenProvider = tokenProvider;
            _httpClientFactory = httpClientFactory;
            _authStateProvider = authStateProvider;
            _logger = logger;
            _jsRuntime = jsRuntime;
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
                    _logger.LogWarning("Token refresh failed, logging user out");

                    // Clear tokens and notify auth state change
                    _tokenProvider.ClearToken();
                    await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
                    await _authStateProvider.MarkUserAsUnauthenticated();
                }
            }

            return response;
        }

        private async Task<bool> TryRefreshTokenAsync()
        {
            try
            {
                // Obtain refresh token from localStorage
                var refreshToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "refreshToken");
                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    _logger.LogWarning("No refresh token found in localStorage");
                    return false;
                }

                using var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5164/") };

                var payload = JsonSerializer.Serialize(new { refreshToken });
                using var content = new StringContent(payload, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("api/auth/refresh", content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Refresh endpoint returned status {StatusCode}", response.StatusCode);
                    return false;
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                var refreshResponse = JsonSerializer.Deserialize<AuthRefreshResponse>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (refreshResponse?.Succeeded == true && !string.IsNullOrEmpty(refreshResponse.AccessToken))
                {
                    _tokenProvider.SetToken(refreshResponse.AccessToken);

                    // Update refresh token if rotated
                    if (!string.IsNullOrEmpty(refreshResponse.RefreshToken))
                    {
                        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "refreshToken", refreshResponse.RefreshToken);
                    }

                    await _authStateProvider.MarkUserAsAuthenticated(refreshResponse.AccessToken, refreshResponse.RefreshToken);
                    return true;
                }

                _logger.LogWarning("Refresh response deserialization failed or unsuccessful");
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
        public string? RefreshToken { get; set; }
        public string? Message { get; set; }
    }
}
