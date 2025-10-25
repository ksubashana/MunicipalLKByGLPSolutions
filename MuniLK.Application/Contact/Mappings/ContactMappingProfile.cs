using MuniLK.Application.Contact.DTOs;
using MuniLK.Domain.Entities;


public static class ContactMappingProfile
{
    public static MuniLK.Domain.Entities.ContactEntities.Contact ToEntity(this CreateContactRequest dto, Guid? tenantId)
    {
        if (dto == null)
        {
            return null;
        }

        return new MuniLK.Domain.Entities.ContactEntities.Contact
        {
            Id = Guid.NewGuid(), // Generate new GUID for a new entity
            TenantId = tenantId,
            FullName = dto.FullName ?? string.Empty,
            NIC = dto.NIC ?? string.Empty,
            Email = dto.Email ?? string.Empty,
            PhoneNumber = dto.PhoneNumber ?? string.Empty,
            AddressLine1 = dto.AddressLine1 ?? string.Empty,
            AddressLine2 = dto.AddressLine2 ?? string.Empty,
            City = dto.City ?? string.Empty,
            District = dto.District ?? string.Empty,
            Province = dto.Province ?? string.Empty,
            PostalCode = dto.PostalCode ?? string.Empty,
            IsActive = true, // Default to active for new contacts
            CreatedAt = DateTime.UtcNow // Set creation timestamp
        };
    }

    public static ContactResponse ToResponse(this MuniLK.Domain.Entities.ContactEntities.Contact entity)
    {
        return new ContactResponse
        {
            Id = entity.Id,
            NationalId = entity.NIC,
            FullName = entity.FullName,
            Address = $"{entity.AddressLine1} {entity.AddressLine2}".Trim(),
            Email = entity.Email,
            PhoneNumber = entity.PhoneNumber
        };
    }

    // Optional: Extension method to update an existing entity from a DTO
    // This can be used in the UpdateContactCommandHandler
    public static void UpdateFromDto(this MuniLK.Domain.Entities.ContactEntities.Contact entity, UpdateContactRequest dto)
    {
        if (entity == null || dto == null)
        {
            return;
        }

        entity.FullName = dto.FullName;
        entity.NIC = dto.NIC;
        entity.Email = dto.Email;
        entity.PhoneNumber = dto.PhoneNumber;
        entity.AddressLine1 = dto.AddressLine1;
        entity.AddressLine2 = dto.AddressLine2;
        entity.City = dto.City;
        entity.District = dto.District;
        entity.Province = dto.Province;
        entity.PostalCode = dto.PostalCode;
        entity.IsActive = dto.IsActive;
        entity.UpdatedAt = DateTime.UtcNow; // Update timestamp
                                            // TenantId should generally not be updated this way
                                            // Id and CreatedAt should not be updated from request DTO
    }
}


