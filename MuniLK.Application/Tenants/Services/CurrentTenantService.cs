
using Microsoft.AspNetCore.Http; // For IHttpContextAccessor
using MuniLK.Application.Generic.Interfaces;
using System;
using System.Security.Claims;

namespace MuniLK.Application.Tenants.Services
{
    /// <summary>
    /// Resolves the current tenant's ID from HTTP context (e.g., headers, claims).
    /// </summary>
    public class CurrentTenantService : ICurrentTenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid? _tenantId;

        public CurrentTenantService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? GetTenantId()
        {

            if (_tenantId.HasValue)
            {
                return _tenantId;
            }

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                // 1. Try to get TenantId from JWT claims (recommended for authenticated API calls)
                var tenantIdClaim = httpContext.User.FindFirst("tenantid"); // Custom claim name
                if (tenantIdClaim != null && Guid.TryParse(tenantIdClaim.Value, out Guid parsedTenantIdFromClaim))
                {
                    _tenantId = parsedTenantIdFromClaim;
                    return _tenantId;
                }

                // 2. Fallback: Try to get TenantId from a custom HTTP header (e.g., for unauthenticated or specific flows)
                if (httpContext.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdHeader))
                {
                    if (Guid.TryParse(tenantIdHeader, out Guid parsedTenantIdFromHeader))
                    {
                        _tenantId = parsedTenantIdFromHeader;
                        return _tenantId;
                    }
                }

                // 3. Fallback: Try to get TenantId from subdomain (if applicable)
                // This requires more complex setup (DNS, routing, and parsing the host)
                // Example: if (httpContext.Request.Host.Host.Contains(".")) { /* parse subdomain */ }
            }

            // If no tenant ID is found, return null.
            // For entities that MUST have a TenantId, this will lead to an exception during SaveChanges.
            // For system operations or logs not tied to a specific tenant, this might be acceptable.
            return null;
        }

        /// <summary>
        /// Allows setting the tenant ID explicitly for non-HTTP contexts (e.g., background jobs, console apps).
        /// This method is typically used by background services or tests where HttpContext is not available.
        /// </summary>
        public void SetTenantId(Guid tenantId)
        {
            _tenantId = tenantId;
        }
    }
}
