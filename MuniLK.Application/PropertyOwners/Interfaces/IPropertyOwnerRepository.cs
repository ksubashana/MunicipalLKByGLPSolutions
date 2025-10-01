using MuniLK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.PropertyOwners.Interfaces
{
    public interface IPropertyOwnerRepository
    {
        Task<PropertyOwner> AddAsync(PropertyOwner entity, CancellationToken cancellationToken = default);
        Task<PropertyOwner?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<PropertyOwner>> GetByPropertyIdAsync(Guid propertyId, CancellationToken cancellationToken = default);
        Task DeleteAsync(PropertyOwner entity, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid propertyId, Guid contactId, Guid? tenantId, CancellationToken cancellationToken = default);

    }
}
