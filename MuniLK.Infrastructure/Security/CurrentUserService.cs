// MuniLK.Infrastructure.Services/CurrentUserService.cs
using Microsoft.AspNetCore.Http;
using MuniLK.Application.Generic.Interfaces;
using System.Linq;
using System.Security.Claims; // Needed for ClaimsPrincipal

namespace MuniLK.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? CurrentUser => _httpContextAccessor.HttpContext?.User;

        public string? UserId => CurrentUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public string? UserName => CurrentUser?.FindFirst(ClaimTypes.Name)?.Value
                                  ?? CurrentUser?.FindFirst(ClaimTypes.Email)?.Value; // Fallback to email if Name isn't set

        public ClaimsPrincipal? User => CurrentUser;

        public bool IsAuthenticated => CurrentUser?.Identity?.IsAuthenticated ?? false;

        public bool IsInRole(string roleName)
        {
            return CurrentUser?.IsInRole(roleName) ?? false;
        }

        public string? GetClaim(string claimType)
        {
            return CurrentUser?.FindFirst(claimType)?.Value;
        }
        public IEnumerable<string> GetRoles()
        {
            return CurrentUser?.FindAll(ClaimTypes.Role).Select(c => c.Value) ?? Enumerable.Empty<string>();
        }

    }
}