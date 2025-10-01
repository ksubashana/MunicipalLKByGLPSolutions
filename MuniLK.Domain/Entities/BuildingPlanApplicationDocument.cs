using System;
using System.ComponentModel.DataAnnotations;
using MuniLK.Domain.Interfaces;

namespace MuniLK.Domain.Entities
{
    /// <summary>
    /// Linking entity between Construction Applications and Documents.
    /// </summary>
    public class BuildingPlanApplicationDocument : IHasTenant
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid BuildingPlanApplicationId { get; set; }
        public BuildingPlanApplication BuildingPlanApplication { get; set; } = null!;

        [Required]
        public Guid DocumentId { get; set; }
        public Document Document { get; set; } = null!;

        [MaxLength(256)]
        public string? LinkContext { get; set; } // e.g. "Site Plan", "Structural Drawings"

        public bool IsPrimary { get; set; } = false;

        public Guid? TenantId { get; set; }

        public DateTime LinkedDate { get; set; } = DateTime.UtcNow;
        public string? LinkedBy { get; set; }
    }
}
