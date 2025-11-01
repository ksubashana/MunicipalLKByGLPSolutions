// MuniLK.API/Controllers/AuthController.cs
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.Generic.DTOs;
using MuniLK.Application.Generic.Interfaces;
using System.Threading.Tasks;

namespace MuniLK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IDataProtectionProvider _dataProtectionProvider;

        public AuthController(IAuthService authService, IDataProtectionProvider dataProtectionProvider)
        {
            _authService = authService;
            _dataProtectionProvider = dataProtectionProvider;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost("RegisterUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            if (!response.Succeeded)
            {
                return BadRequest(response.Errors);
            }
            return Ok(response); // Contains UserId now
        }

        /// <summary>
        /// Authenticates a user and returns JWT tokens.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            if (!response.Succeeded)
            {
                return Unauthorized(response.Errors);
            }

            // Set refresh token as secure httpOnly cookie
            if (!string.IsNullOrEmpty(response.RefreshToken))
            {
                var protector = _dataProtectionProvider.CreateProtector("RefreshTokens");
                var protectedRefreshToken = protector.Protect(response.RefreshToken);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Use HTTPS in production
                    SameSite = SameSiteMode.None, // changed from Strict to None to allow cross-origin Blazor client
                    Expires = DateTimeOffset.UtcNow.AddHours(12)
                };

                Response.Cookies.Append("refresh_token", protectedRefreshToken, cookieOptions);
            }

            return Ok(new {
                response.Succeeded,
                response.AccessToken,
                response.RefreshToken,
                response.Message,
                response.UserId
            });
        }

        /// <summary>
        /// Refreshes an access token using a refresh token from cookie or X-Refresh-Token header fallback.
        /// </summary>
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken()
        {
            string? rawRefreshToken = null;
            // Try cookie first
            if (Request.Cookies.TryGetValue("refresh_token", out var protectedRefreshToken))
            {
                try
                {
                    var protector = _dataProtectionProvider.CreateProtector("RefreshTokens");
                    rawRefreshToken = protector.Unprotect(protectedRefreshToken);
                }
                catch
                {
                    Response.Cookies.Delete("refresh_token");
                    rawRefreshToken = null; // fall through to header/body fallback
                }
            }

            // Fallback: custom header containing plain refresh token (not protected). Client can store it.
            if (string.IsNullOrEmpty(rawRefreshToken) && Request.Headers.TryGetValue("X-Refresh-Token", out var headerToken))
            {
                rawRefreshToken = headerToken.ToString();
            }

            if (string.IsNullOrEmpty(rawRefreshToken))
            {
                return Unauthorized(new { Errors = new[] { "Refresh token not found." } });
            }

            var response = await _authService.RefreshTokenAsync(rawRefreshToken);
            if (!response.Succeeded)
            {
                Response.Cookies.Delete("refresh_token");
                return Unauthorized(response.Errors);
            }

            // Rotate cookie if we used cookie previously or want to set new one
            if (!string.IsNullOrEmpty(response.RefreshToken))
            {
                try
                {
                    var protector = _dataProtectionProvider.CreateProtector("RefreshTokens");
                    var newProtectedRefreshToken = protector.Protect(response.RefreshToken);
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTimeOffset.UtcNow.AddHours(12)
                    };
                    Response.Cookies.Append("refresh_token", newProtectedRefreshToken, cookieOptions);
                }
                catch { /* ignore */ }
            }

            return Ok(new {
                response.Succeeded,
                response.AccessToken,
                response.RefreshToken,
                Message = "Token refreshed successfully",
                response.UserId
            });
        }

        /// <summary>
        /// Logs out the user by clearing the refresh token cookie.
        /// </summary>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Logout()
        {
            // Clear the refresh token cookie
            Response.Cookies.Delete("refresh_token");
            return Ok(new { Message = "Logged out successfully" });
        }

        /// <summary>
        /// Removes a user from the identity system by their User ID.
        /// </summary>
        /// <param name="request">The request containing the User ID to delete.</param>
        /// <returns>A status indicating success or failure of the deletion.</returns>
        [HttpDelete("RemoveUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Added for clarity if user not found
        public async Task<IActionResult> RemoveUser([FromBody] DeleteUserRequest request)
        {
            var response = await _authService.RemoveUserAsync(request);
            if (!response.Succeeded)
            {
                // If user not found, return 404. Otherwise, 400 for other errors.
                if (response.Errors != null && response.Errors.Contains("User not found."))
                {
                    return NotFound(response.Errors);
                }
                return BadRequest(response.Errors);
            }
            return Ok(new { response.Message, response.UserId }); // Return a success message
        }
    }
}
