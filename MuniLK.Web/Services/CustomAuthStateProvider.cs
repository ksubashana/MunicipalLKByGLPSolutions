using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MuniLK.Web.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly TokenProvider _tokenProvider;
        private bool _isPrerendering = true;
        private readonly IConfiguration _configuration; // Add IConfiguration field
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly string _jwtKey;
        // Configuration values should not be hardcoded. 
        // For a production app, fetch these from a secure source.
        // For a client-side app, these are typically stored in appsettings.json or similar config.


        public CustomAuthStateProvider(IJSRuntime jsRuntime, TokenProvider tokenProvider, IConfiguration configuration)
        {
            _jsRuntime = jsRuntime;
            _tokenProvider = tokenProvider;
            _configuration = configuration;
            _jwtIssuer = _configuration["Jwt:Issuer"] ?? "http://localhost:5164";
            _jwtAudience = _configuration["Jwt:Audience"] ?? "http://localhost:5116";
            _jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing in configuration");
        }

        public void NotifyPrerenderComplete()
        {
            _isPrerendering = false;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            ClaimsPrincipal user = new(identity);

            try
            {
                if (!_isPrerendering)
                {
                    var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

                    if (!string.IsNullOrEmpty(token))
                    {
                        var principal = ValidateAndParseToken(token);
                        if (principal != null)
                        {
                            user = principal;
                            _tokenProvider.SetToken(token); // Store token in memory

                        }
                        else
                        {
                            _tokenProvider.ClearToken(); // Token is invalid, clear it

                        }
                    }
                    else
                    {
                        _tokenProvider.ClearToken();

                    }
                }
                // If in prerendering mode, we deliberately return an unauthenticated user.
                // The client-side will then re-render with the correct state.
            }
            catch
            {
                // During prerender, JS interop will fail. Just return anonymous user.
                identity = new ClaimsIdentity();
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));

            return new AuthenticationState(user);
        }

        public async Task MarkUserAsAuthenticated(string token)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
            _tokenProvider.SetToken(token);

            var principal = ValidateAndParseToken(token);
            var user = principal ?? new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            _tokenProvider.ClearToken();

            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
        }

        public async Task MarkUserAsUnauthenticated()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            _tokenProvider.ClearToken();

            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
        }

        /// <summary>
        /// Validates the JWT and returns a ClaimsPrincipal if valid.
        /// Returns null otherwise.
        /// </summary>
        private ClaimsPrincipal? ValidateAndParseToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtIssuer,
                ValidAudience = _jwtAudience,
                IssuerSigningKey = key,
            };

            try
            {
                // The handler will throw an exception if the token is invalid.
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                // Check if the token is an HmacSha256 JWT
                if (validatedToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch (Exception)
            {
                // Return null for any validation failure (e.g., expired token, bad signature)
                return null;
            }
        }
        public async Task<string?> GetUserRoleAsync()
        {
            var state = await GetAuthenticationStateAsync();
            return state.User.FindFirst(ClaimTypes.Role)?.Value;
        }

        public async Task<string?> GetUserIdAsync()
        {
            var state = await GetAuthenticationStateAsync();
            return state.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? state.User.FindFirst("sub")?.Value;
        }

        public async Task<string?> GetUserNameAsync()
        {
            var state = await GetAuthenticationStateAsync();
            return state.User.Identity?.Name;
        }

        public async Task<bool> IsInRoleAsync(string role)
        {
            bool isInRole = false;
            var state = await GetAuthenticationStateAsync();
            isInRole= state.User.IsInRole(role);
            return isInRole;
        }
    }
}