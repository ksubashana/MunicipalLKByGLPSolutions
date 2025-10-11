using System;
using System.ComponentModel.DataAnnotations;
using MuniLK.Domain.Interfaces;

namespace MuniLK.Domain.Entities
{
    /// <summary>
    /// Represents an individual option within a group (e.g., "Fire Clearance", "Environmental Clearance")
    /// </summary>
    public class OptionItem : IHasTenant
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid GroupId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public Guid? TenantId { get; set; }

        // Navigation property
        public OptionGroup? OptionGroup { get; set; }
    }
}
