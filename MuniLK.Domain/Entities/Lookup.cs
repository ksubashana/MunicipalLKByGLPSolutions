// MuniLK.Domain/Entities/Lookup.cs
using System;
using System.ComponentModel.DataAnnotations;
using MuniLK.Domain.Interfaces; // For IHasTenant

namespace MuniLK.Domain.Entities
{
    /// <summary>
    /// Represents a customizable lookup value in the system.
    /// Belongs to a specific LookupCategory and can be global or tenant-specific.
    /// </summary>
    public class Lookup : IHasTenant // Implements IHasTenant, but TenantId is nullable here
    {
        [Key]
        public Guid Id { get; set; } // Primary key for the lookup entry

        /// <summary>
        /// Foreign key to the LookupCategory table.
        /// </summary>
        [Required]
        public Guid LookupCategoryId { get; set; }

        /// <summary>
        /// Navigation property to the associated LookupCategory.
        /// </summary>
        public LookupCategory LookupCategory { get; set; } = null!; // Required navigation property

        /// <summary>
        /// The actual value of the lookup item (e.g., "Primary Owner", "Residential", "Permanent").
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Optional: A display order for the value within its category.
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// The TenantId this lookup value belongs to.
        /// NULL indicates a global (shared) lookup value.
        /// </summary>
        public Guid? TenantId { get; set; } // Nullable TenantId

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
    }
}
