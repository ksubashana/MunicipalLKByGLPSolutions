// MuniLK.Domain/Entities/LookupCategory.cs
using System;
using System.ComponentModel.DataAnnotations;
using MuniLK.Domain.Interfaces; // Assuming IHasTenant is here

namespace MuniLK.Domain.Entities
{
    /// <summary>
    /// Represents a category for lookup values (e.g., "PropertyType", "OwnershipType").
    /// Can be global (TenantId is NULL) or tenant-specific.
    /// </summary>
    public class LookupCategory : IHasTenant // Lookup categories can also be tenant-specific
    {
        [Key]
        public Guid Id { get; set; } // Primary key for the lookup category

        /// <summary>
        /// The unique name of the category (e.g., "PropertyType", "RoadAccessType").
        /// This is often used as a programmatic key.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// A human-readable display name for the category (e.g., "Property Types", "Types of Road Access").
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Optional: A description for the category.
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Optional: A display order for categories in a list.
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// The TenantId this lookup category belongs to.
        /// NULL indicates a global (shared) lookup category.
        /// </summary>
        public Guid? TenantId { get; set; } // Nullable TenantId for global/tenant-specific categories

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
    }
}
