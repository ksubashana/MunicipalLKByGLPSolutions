using Microsoft.EntityFrameworkCore;
using MuniLK.Application.PropertyOwners.Interfaces;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Infrastructure.PropertyOwners
{
    public class PropertyOwnerRepository : IPropertyOwnerRepository
    {
        private readonly MuniLKDbContext _context;

        public PropertyOwnerRepository(MuniLKDbContext context)
        {
            _context = context;
        }

        public async Task<PropertyOwner> AddAsync(PropertyOwner entity, CancellationToken cancellationToken = default)
        {
            await _context.PropertyOwners.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<PropertyOwner?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.PropertyOwners
                .Include(po => po.Contact)
                .Include(po => po.Property)
                .FirstOrDefaultAsync(po => po.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<PropertyOwner>> GetByPropertyIdAsync(Guid propertyId, CancellationToken cancellationToken = default)
        {
            return await _context.PropertyOwners
                .Include(po => po.Contact) // Include the linked Contact
                .Where(po => po.PropertyId == propertyId)
                .ToListAsync(cancellationToken);
        }

        public async Task DeleteAsync(PropertyOwner entity, CancellationToken cancellationToken = default)
        {
            _context.PropertyOwners.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid propertyId, Guid contactId, Guid? tenantId, CancellationToken cancellationToken = default)
        {
            return await _context.PropertyOwners
                .AnyAsync(po => po.PropertyId == propertyId
                             && po.ContactId == contactId
                             && po.TenantId == tenantId, cancellationToken);
        }

    }
}
