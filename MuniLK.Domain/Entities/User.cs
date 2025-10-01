using System;
using MuniLK.Domain.Interfaces; // For IHasTenant

namespace MuniLK.Domain.Entities
{
    /// <summary>
    /// Represents the core domain user entity.
    /// This is separate from ASP.NET Core Identity's IdentityUser.
    /// </summary>
    public class User : IHasTenant
    {
        public Guid Id { get; set; } // Matches IdentityUser.Id type (string converted to Guid)
        public Guid? TenantId { get; set; } // For multi-tenancy
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        // Link to Contact for personal info
        public Guid? ContactId { get; set; }
        public MuniLK.Domain.Entities.ContactEntities.Contact? Contact { get; set; }

        public bool IsActive { get; set; } = true;

        // Optional: business-specific fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}