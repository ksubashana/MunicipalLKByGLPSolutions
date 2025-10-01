// MuniLK.Domain/Entities/ClientConfiguration.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MuniLK.Domain.Interfaces; // Ensure this using is present

namespace MuniLK.Domain.Entities
{
    /// <summary>
    /// Represents client-specific configuration settings.
    /// The actual configuration data is stored as a JSON string.
    /// </summary>
    public class ClientConfiguration : IHasTenant
    {
        [Key]
        public Guid Id { get; set; } // Primary key for the configuration entry

        // Foreign key to the Tenant (required for multi-tenancy)
        public Guid? TenantId { get; set; }

        /// <summary>
        /// A unique key or name for this specific configuration set within a tenant.
        /// E.g., "GeneralSettings", "FeatureToggles", "EmailSettings".
        /// This allows a tenant to have multiple types of configurations.
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string ConfigKey { get; set; } = string.Empty;

        /// <summary>
        /// The actual configuration data stored as a JSON string.
        /// This provides flexibility for complex or evolving settings.
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string ConfigJson { get; set; } = string.Empty;

        /// <summary>
        /// Optional: Timestamp for when the configuration was last updated.
        /// </summary>
        public DateTimeOffset LastUpdated { get; set; } = DateTimeOffset.UtcNow;

        // You might add an index to (TenantId, ConfigKey) for efficient lookups
        // This can be done in OnModelCreating in DbContext or via a migration.
    }
}
