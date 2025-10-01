// MuniLK.Application/Services/ICurrentTenantService.cs
using System;

namespace MuniLK.Application.Generic.Interfaces
{
    /// <summary>
    /// Defines a service to resolve the current tenant's ID.
    /// </summary>
    public interface ICurrentTenantService
    {
        Guid? GetTenantId();
        void SetTenantId(Guid tenantId);

    }
}