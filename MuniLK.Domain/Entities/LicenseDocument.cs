using System;
using System.ComponentModel.DataAnnotations;
using MuniLK.Domain.Interfaces;

namespace MuniLK.Domain.Entities
{
    /// <summary>
    /// Linking entity for many-to-many relationship between License and Document.
    /// </summary>
    public class LicenseDocument : IHasTenant
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid LicenseId { get; set; }
        public License License { get; set; } = null!;

        [Required]
        public Guid DocumentId { get; set; }
        public Document Document { get; set; } = null!;

        [MaxLength(256)]
        public string? LinkContext { get; set; }

        public bool IsPrimary { get; set; } = false;

        public Guid? TenantId { get; set; }

        public DateTime LinkedDate { get; set; } = DateTime.UtcNow;
        public string? LinkedBy { get; set; }
    }
}