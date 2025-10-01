using MuniLK.Domain.Entities;
using MuniLK.Domain.Interfaces;

namespace MuniLK.Domain.Entities.ContactEntities
{
    public class Contact : IHasTenant // Contact is a tenant-scoped entity
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    public string FullName { get; set; }

    public string NIC { get; set; } // National ID (like 200034501234)

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }

    public string City { get; set; }

    public string District { get; set; }

    public string Province { get; set; }

    public string PostalCode { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation property for properties they own or apply for
    public ICollection<PropertyOwner> PropertyOwners { get; set; } = new List<PropertyOwner>();
}
}



