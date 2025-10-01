// MuniLK.Infrastructure/Security/JwtTokenService.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MuniLK.Domain.Interfaces; // For ITokenService
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Infrastructure.Security
{
    /// <summary>
    /// Implementation of ITokenService for generating and validating JWTs.
    /// </summary>
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;

        public JwtTokenService(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        /// <summary>
        /// Generates a JWT access token for the given domain user.
        /// Includes user ID, username, roles, and TenantId as claims.
        /// </summary>
        public async Task<string> GenerateAccessTokenAsync(Domain.Entities.User domainUser)
        {
            // Retrieve the IdentityUser to get roles and potentially other Identity-specific claims
            var identityUser = await _userManager.FindByIdAsync(domainUser.Id.ToString());
            if (identityUser == null)
            {
                throw new InvalidOperationException($"IdentityUser not found for domain user ID: {domainUser.Id}");
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, identityUser.Id), // Subject (user ID)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID
                new Claim(ClaimTypes.NameIdentifier, identityUser.Id), // Standard claim for user ID
                new Claim(ClaimTypes.Name, identityUser.UserName ?? identityUser.Email ?? domainUser.Username) // Standard claim for username
            };

            // Add roles as claims
            var roles = await _userManager.GetRolesAsync(identityUser);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Add TenantId as a custom claim
            if (domainUser.TenantId != Guid.Empty) // Assuming Guid.Empty is not a valid tenant ID
            {
                claims.Add(new Claim("tenantid", domainUser.TenantId.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured.")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpirationMinutes"] ?? "15"));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generates a refresh token (JWT) with 12-hour expiry for the given domain user.
        /// </summary>
        public Task<string> GenerateRefreshTokenAsync(Domain.Entities.User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured.")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("email", user.Email),
                new Claim("tenantid", user.TenantId.ToString()),
                new Claim("token_type", "refresh")
            };

            // Refresh token expires in 12 hours
            var expires = DateTime.UtcNow.AddHours(12);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"], 
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        /// <summary>
        /// Retrieves claims principal from an expired token for refresh token validation.
        /// </summary>
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, // We don't validate audience for expired tokens here
                ValidateIssuer = false,   // We don't validate issuer for expired tokens here
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured."))),
                ValidateLifetime = false  // IMPORTANT: Do not validate lifetime here for expired tokens
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token: Algorithm mismatch or not a JWT.");
            }

            return principal;
        }
    }
}
