using System;
using System.ComponentModel.DataAnnotations;
using MuniLK.Domain.Interfaces;

namespace MuniLK.Domain.Entities
{
    /// <summary>
    /// Represents a multi-select/checkbox selection for any entity type (e.g., SiteInspection, Permit)
    /// </summary>
    public class EntityOptionSelection : IHasTenant
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid EntityId { get; set; }

        [Required]
        [MaxLength(100)]
        public string EntityType { get; set; } = string.Empty;

        [Required]
        public Guid OptionItemId { get; set; }

        [Required]
        public Guid ModuleId { get; set; }

        public Guid? TenantId { get; set; }

        // Navigation property
        public OptionItem? OptionItem { get; set; }
    }
}
