// MuniLK.Domain/Entities/Tenant.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace MuniLK.Domain.Entities
{
    /// <summary>
    /// Represents a client (tenant) in the multi-tenant system.
    /// This table stores metadata about each tenant and their unique TenantId.
    /// </summary>
    public class Tenant
    {
        [Key]
        public Guid TenantId { get; set; } // Primary Key - The unique identifier for the tenant

        [Required]
        [MaxLength(256)]
        public string Name { get; set; } = string.Empty; // Human-readable name of the municipality/client

        [MaxLength(256)]
        public string? Subdomain { get; set; } // Optional: e.g., "municipalitya" if using subdomains

        [MaxLength(256)]
        public string? ContactEmail { get; set; } // Main contact email for the tenant

        public bool IsActive { get; set; } = true; // Indicates if the tenant is active

        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow; // When the tenant was onboarded

        public DateTimeOffset? LastModifiedDate { get; set; } // When tenant details were last updated

        // Add other tenant-specific metadata as needed (e.g., SubscriptionPlan, MaxUsers, FeaturesEnabled)
    }
}
