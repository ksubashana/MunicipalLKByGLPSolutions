// MuniLK.API/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.Generic.DTOs;
using MuniLK.Application.Generic.Interfaces;
using System.Threading.Tasks;
using MuniLK.Domain.Interfaces; // added for store
using MuniLK.Infrastructure.Security; // for RefreshTokenStore.Hash
using System;
using System.Linq;

namespace MuniLK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRefreshTokenStore _refreshStore;

        public AuthController(IAuthService authService, IRefreshTokenStore refreshStore)
        {
            _authService = authService;
            _refreshStore = refreshStore;
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

            if (!string.IsNullOrEmpty(response.RefreshToken) && Guid.TryParse(response.UserId, out var userIdGuid))
            {
                var hashed = RefreshTokenStore.Hash(response.RefreshToken);
                await _refreshStore.AddAsync(new Domain.Entities.RefreshToken
                {
                    UserId = userIdGuid,
                    TenantId = request.TenantId,
                    TokenHash = hashed,
                    ExpiresUtc = DateTime.UtcNow.AddHours(8) // 8 hour lifetime
                });
                await _refreshStore.SaveChangesAsync();
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

            if (!string.IsNullOrEmpty(response.RefreshToken) && Guid.TryParse(response.UserId, out var userIdGuid))
            {
                var hashed = RefreshTokenStore.Hash(response.RefreshToken);
                await _refreshStore.AddAsync(new Domain.Entities.RefreshToken
                {
                    UserId = userIdGuid,
                    TokenHash = hashed,
                    ExpiresUtc = DateTime.UtcNow.AddHours(8)
                });
                await _refreshStore.SaveChangesAsync();
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
        /// Refreshes an access token. Token can be supplied in body, X-Refresh-Token header or refreshToken cookie.
        /// </summary>
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest? body)
        {
            // Allow multiple input mechanisms so caller need not post JSON explicitly
            var rawToken = body?.RefreshToken;
            if (string.IsNullOrWhiteSpace(rawToken)) rawToken = Request.Headers["X-Refresh-Token"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(rawToken)) rawToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrWhiteSpace(rawToken)) return Unauthorized(new { Errors = new[] { "Refresh token missing." } });

            var hashed = RefreshTokenStore.Hash(rawToken);
            var existing = await _refreshStore.GetByHashAsync(hashed);
            if (existing == null || !existing.IsActive) return Unauthorized(new { Errors = new[] { "Invalid refresh token." } });

            var response = await _authService.RefreshTokenAsync(rawToken);
            if (!response.Succeeded)
            {
                await _refreshStore.RevokeAsync(existing.Id);
                await _refreshStore.SaveChangesAsync();
                return Unauthorized(response.Errors);
            }

            // Rotate token
            if (!string.IsNullOrEmpty(response.RefreshToken))
            {
                var newHashed = RefreshTokenStore.Hash(response.RefreshToken);
                var newToken = new Domain.Entities.RefreshToken
                {
                    UserId = existing.UserId,
                    TenantId = existing.TenantId,
                    TokenHash = newHashed,
                    ExpiresUtc = DateTime.UtcNow.AddHours(8)
                };
                await _refreshStore.AddAsync(newToken);
                await _refreshStore.RevokeAsync(existing.Id, newToken.Id);
                await _refreshStore.SaveChangesAsync();
            }

            return Ok(new {
                response.Succeeded,
                response.AccessToken,
                response.RefreshToken,
                response.UserId
            });
        }

        /// <summary>
        /// Logs out the user by clearing the refresh token cookie.
        /// </summary>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest body)
        {
            if (string.IsNullOrWhiteSpace(body.RefreshToken)) return Ok(new { Message = "Logged out." });
            var hashed = RefreshTokenStore.Hash(body.RefreshToken);
            var existing = await _refreshStore.GetByHashAsync(hashed);
            if (existing != null && existing.IsActive)
            {
                await _refreshStore.RevokeAsync(existing.Id);
                await _refreshStore.SaveChangesAsync();
            }
            return Ok(new { Message = "Logged out." });
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
