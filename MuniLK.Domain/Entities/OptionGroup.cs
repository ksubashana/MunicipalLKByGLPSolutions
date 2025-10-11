using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MuniLK.Domain.Interfaces;

namespace MuniLK.Domain.Entities
{
    /// <summary>
    /// Represents a group of related options (e.g., "Clearance Types", "Inspection Categories")
    /// </summary>
    public class OptionGroup : IHasTenant
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public Guid? TenantId { get; set; }

        // Navigation property
        public ICollection<OptionItem> OptionItems { get; set; } = new List<OptionItem>();
    }
}
