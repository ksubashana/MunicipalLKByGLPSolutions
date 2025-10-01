using System;
using System.ComponentModel.DataAnnotations;
using MuniLK.Domain.Entities; // To access LicenseType enum

namespace MuniLK.Application.LicensesLK.DTOs
{
    /// <summary>
    /// DTO for creating a new License.
    /// Excludes Id and TenantId as these are generated/assigned by the system.
    /// </summary>
    public class CreateLicenseRequest
    {
        [Required(ErrorMessage = "License Number is required.")]
        [MaxLength(50)]
        public string LicenseNumber { get; set; } = default!;

        [Required(ErrorMessage = "License Type is required.")]
        [EnumDataType(typeof(LicenseType), ErrorMessage = "Invalid License Type.")]
        public LicenseType Type { get; set; }

        [Required(ErrorMessage = "Professional Name is required.")]
        [MaxLength(250)]
        public string ProfessionalName { get; set; } = default!;

        [Required(ErrorMessage = "Profession is required.")]
        [MaxLength(100)]
        public string Profession { get; set; } = default!;

        [Required(ErrorMessage = "NIC or BRN is required.")]
        [MaxLength(50)]
        public string NICOrBRN { get; set; } = default!; // National ID or Business Registration Number

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(500)]
        public string Address { get; set; } = default!;

        [Required(ErrorMessage = "Municipality is required.")]
        [MaxLength(100)]
        public string Municipality { get; set; } = default!;

        [Required(ErrorMessage = "Issue Date is required.")]
        public DateTime IssueDate { get; set; }

        [Required(ErrorMessage = "Expiry Date is required.")]
        public DateTime ExpiryDate { get; set; }

        [Required(ErrorMessage = "Fee is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fee must be greater than zero.")]
        public decimal Fee { get; set; }

        public bool IsActive { get; set; } = true; // Default to active

        [MaxLength(1000)]
        public string? Remarks { get; set; }
    }
}
