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
            // Use FindAsync for primary key lookup, which is optimized.
            // If you need to include navigation properties, use .Include()
            //Guid id = Guid.Parse(id2.ToString());

            return await _context.Contacts
                                 .Include(c => c.PropertyOwners) // Example: include related property owners
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<MuniLK.Domain.Entities.ContactEntities.Contact> GetByIdAsync(Guid id)
        {
            // Use FindAsync for primary key lookup, which is optimized.

            return await _context.Contacts.FindAsync(id);

        }
        public async Task AddAsync(MuniLK.Domain.Entities.ContactEntities.Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }
            await _context.Contacts.AddAsync(contact);
            //await _context.SaveChangesAsync(); unit of work
        }

        public async Task UpdateAsync(MuniLK.Domain.Entities.ContactEntities.Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }
            // Attach the entity if it's not already tracked and mark as modified
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
            // Optional: Handle case where contact is not found (e.g., throw NotFoundException)
        }

        public async Task<IEnumerable<MuniLK.Domain.Entities.ContactEntities.Contact>> GetAllAsync()
        {
            // Use ToListAsync() to execute the query and return all contacts
            // You might want to include navigation properties here too if always needed
            return await _context.Contacts
                                 .Include(c => c.PropertyOwners) // Example: include related property owners
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
