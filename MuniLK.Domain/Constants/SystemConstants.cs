// MuniLK.Domain/Constants/SystemConstants.cs
using System;

namespace MuniLK.Domain.Constants
{
    /// <summary>
    /// Defines system-wide constant values.
    /// </summary>
    public static class SystemConstants
    {
        /// <summary>
        /// A special TenantId used to identify global or system-level data
        /// that does not belong to any specific client tenant.
        /// </summary>
        public static readonly Guid SystemTenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    }
}
