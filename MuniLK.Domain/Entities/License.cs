using MuniLK.Domain.Interfaces;

namespace MuniLK.Domain.Entities
{
    public enum LicenseType
    {
        Trade,
        Professional,
        Construction,
        Event,
        Other
    }

    public class License : IHasTenant
    {
        public Guid? Id { get; set; }
        public Guid? TenantId { get; set; }
        public string LicenseNumber { get; set; } = default!;
        public LicenseType Type { get; set; }
        public string ProfessionalName { get; set; } = default!;
        public string Profession { get; set; } = default!;
        public string NICOrBRN { get; set; } = default!; // National ID or Business Registration Number
        public string Address { get; set; } = default!;
        public string Municipality { get; set; } = default!;
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Fee { get; set; }
        public bool IsActive { get; set; }
        public string? Remarks { get; set; }

    }
}