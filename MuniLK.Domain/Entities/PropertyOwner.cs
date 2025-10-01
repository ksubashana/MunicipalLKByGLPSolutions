using MuniLK.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Domain.Entities
{
    /// <summary>
    /// Represents an association between a Property and a Contact, indicating ownership.
    /// This acts as a join entity in a many-to-many relationship with additional attributes.
    /// </summary>
    public class PropertyOwner : IHasTenant // PropertyOwner is also a tenant-scoped entity
    {
        [Key]
        public Guid Id { get; set; } // Primary key for the PropertyOwner join entity itself

        public Guid? TenantId { get; set; } // TenantId for multi-tenancy, inherited from IHasTenant

        // Foreign Key to Property
        [Required]
        public Guid? PropertyId { get; set; }

        // Navigation property to the associated Property
        public Property Property { get; set; } = null!; // Required navigation property

        // Foreign Key to Contact
        [Required]
        public Guid ContactId { get; set; }

        // Navigation property to the associated Contact (the actual owner details)
        public MuniLK.Domain.Entities.ContactEntities.Contact Contact { get; set; } = null!; // Required navigation property

        /// <summary>
        /// Describes the type of ownership (e.g., "Primary Owner", "Co-owner", "Legal Representative").
        /// </summary>
        [Required]
        [MaxLength(50)] // Adjust max length as needed
        public string OwnershipType { get; set; } = string.Empty;

        // Optional: Metadata for the association itself
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [MaxLength(256)]
        public string? CreatedBy { get; set; }
    }
}
