using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuniLK.Domain.Entities;

namespace MuniLK.Applications.Interfaces
{
    public interface IContactRepository
    {
        Task<MuniLK.Domain.Entities.ContactEntities.Contact> GetByIdAsync(Guid id);
        Task AddAsync(MuniLK.Domain.Entities.ContactEntities.Contact contact);
        Task UpdateAsync(MuniLK.Domain.Entities.ContactEntities.Contact contact);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<MuniLK.Domain.Entities.ContactEntities.Contact>> GetAllAsync();
        Task<IEnumerable<MuniLK.Domain.Entities.ContactEntities.Contact>> SearchAsync(string query);
        Task<MuniLK.Domain.Entities.ContactEntities.Contact?> GetByUniqueFieldsAsync(string? nic, string? email, string? phoneNumber, Guid? tenantId, CancellationToken ct);

    }

}
