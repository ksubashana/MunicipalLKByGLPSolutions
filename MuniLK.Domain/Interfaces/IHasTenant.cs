// MuniLK.Domain/Interfaces/IHasTenant.cs
using System;

namespace MuniLK.Domain.Interfaces
{
    /// <summary>
    /// Interface for entities that belong to a specific tenant.
    /// </summary>
    public interface IHasTenant
    {
        /// <summary>
        /// Gets or sets the unique identifier of the tenant this entity belongs to.
        /// </summary>
        Guid? TenantId { get; set; }
    }
}
