using Microsoft.EntityFrameworkCore;
using MuniLK.Applications.Interfaces;
using MuniLK.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuniLK.Domain.Entities.ContactEntities;

namespace MuniLK.Infrastructure.Contact
{
    public class ContactRepository : IContactRepository
    {
        private readonly MuniLKDbContext _context; // Use your actual DbContext name

        public ContactRepository(MuniLKDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<MuniLK.Domain.Entities.ContactEntities.Contact> GetByIdWithPropOwnersAsync(Guid id)
        {
            return await _context.Contacts
                                 .Include(c => c.PropertyOwners)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<MuniLK.Domain.Entities.ContactEntities.Contact> GetByIdAsync(Guid id)
        {
            // Bypass global/tenant query filters to ensure retrieval by Id.
            return await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task AddAsync(MuniLK.Domain.Entities.ContactEntities.Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }
            await _context.Contacts.AddAsync(contact);
        }

        public async Task UpdateAsync(MuniLK.Domain.Entities.ContactEntities.Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var contactToDelete = await _context.Contacts.FindAsync(id);
            if (contactToDelete != null)
            {
                _context.Contacts.Remove(contactToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MuniLK.Domain.Entities.ContactEntities.Contact>> GetAllAsync()
        {
            return await _context.Contacts
                                 .Include(c => c.PropertyOwners)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<MuniLK.Domain.Entities.ContactEntities.Contact>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<MuniLK.Domain.Entities.ContactEntities.Contact>();

            query = query.ToLower();

            return await _context.Contacts
                .Where(c =>
                    (!string.IsNullOrEmpty(c.FullName) && c.FullName.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(c.NIC) && c.NIC.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(c.AddressLine1) && c.AddressLine1.ToLower().Contains(query))||
                    (!string.IsNullOrEmpty(c.Email) && c.Email.ToLower().Contains(query))||
                    (!string.IsNullOrEmpty(c.PhoneNumber) && c.PhoneNumber.ToLower().Contains(query))||
                    (!string.IsNullOrEmpty(c.AddressLine2) && c.AddressLine2.ToLower().Contains(query)))
                .ToListAsync();
        }

        public async Task<MuniLK.Domain.Entities.ContactEntities.Contact?> GetByUniqueFieldsAsync(string? nic, string? email, string? phoneNumber, Guid? tenantId, CancellationToken ct)
        {
            return await _context.Contacts
                .Where(c => c.TenantId == tenantId)
                .Where(c => (!string.IsNullOrEmpty(nic) && c.NIC == nic)
                         || (!string.IsNullOrEmpty(email) && c.Email == email)
                         || (!string.IsNullOrEmpty(phoneNumber) && c.PhoneNumber == phoneNumber))
                .FirstOrDefaultAsync(ct);
        }

    }
}
