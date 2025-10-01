using MuniLK.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Domain.Entities
{
    public class DocumentLink : IHasTenant
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid DocumentId { get; set; }
        public Document Document { get; set; } = null!;
        [Required]
        public Guid ModuleId { get; set; }
        public Module Module { get; set; } = null!;

        [Required]
        public Guid EntityId { get; set; } // The primary key of the linked entity

        [MaxLength(256)]
        public string? LinkContext { get; set; } // Optional: "Photos", "Blueprints", etc.

        public bool IsPrimary { get; set; } = false;

        public Guid? TenantId { get; set; }

        public DateTime LinkedDate { get; set; } = DateTime.UtcNow;
        public string? LinkedBy { get; set; }
    }

}
