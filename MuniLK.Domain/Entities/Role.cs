// MuniLK.Domain/Entities/Role.cs
using System;

namespace MuniLK.Domain.Entities
{
    /// <summary>
    /// Represents the core domain role entity.
    /// </summary>
    public class Role
    {
        public Guid Id { get; set; } // Matches IdentityRole.Id type
        public string Name { get; set; } = string.Empty;
        // Add other domain-specific role properties if needed
    }
}