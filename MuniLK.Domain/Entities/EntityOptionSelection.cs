using System;
using System.ComponentModel.DataAnnotations;
using MuniLK.Domain.Interfaces;

namespace MuniLK.Domain.Entities
{
    /// <summary>
    /// Represents a multi-select/checkbox selection for any entity type (e.g., SiteInspection, Permit)
    /// Stores selected lookup values (LookupId) scoped by Entity + EntityType + Module + LookupCategoryName.
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

        /// <summary>
        /// Programmatic lookup category name this selection belongs to (e.g. ClearanceTypes, InspectionReports).
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string LookupCategoryName { get; set; } = string.Empty;

        /// <summary>
        /// FK pointing to Lookup.Id
        /// </summary>
        [Required]
        public Guid LookupId { get; set; }

        /// <summary>
        /// Module context (required for scoping selections)
        /// </summary>
        public Guid ModuleId { get; set; }

        public Guid? TenantId { get; set; }
    }
}
