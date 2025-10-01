using MuniLK.Application.LicensesLK.DTOs; // Assuming DTOs folder for License
using MuniLK.Domain.Entities; // To access the License entity
using System;

namespace MuniLK.Application.LicensesLK.Mappings
{
    /// <summary>
    /// Provides static extension methods for mapping License DTOs to License entities.
    /// </summary>
    public static class LicenseMappingProfile
    {
        /// <summary>
        /// Maps a CreateLicenseRequest DTO to a License entity,
        /// generating a new GUID for the Id and assigning the TenantId.
        /// </summary>
        /// <param name="dto">The CreateLicenseRequest DTO.</param>
        /// <param name="tenantId">The TenantId to associate with the License.</param>
        /// <returns>A new License entity.</returns>
        public static License ToEntity(this CreateLicenseRequest dto, Guid? tenantId)
        {
            // Generate a new GUID for the primary key of the License entity.
            var newLicenseId = Guid.NewGuid();

            return new License
            {
                Id = newLicenseId, 
                TenantId = tenantId,
                LicenseNumber = dto.LicenseNumber,
                Type = dto.Type,
                ProfessionalName = dto.ProfessionalName,
                Profession = dto.Profession,
                NICOrBRN = dto.NICOrBRN,
                Address = dto.Address,
                Municipality = dto.Municipality,
                IssueDate = dto.IssueDate,
                ExpiryDate = dto.ExpiryDate,
                Fee = dto.Fee,
                IsActive = dto.IsActive,
                Remarks = dto.Remarks
            };
        }
    }
}
