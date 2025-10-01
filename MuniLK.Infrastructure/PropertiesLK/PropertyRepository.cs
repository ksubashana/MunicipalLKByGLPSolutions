using Microsoft.EntityFrameworkCore;
using MuniLK.Application.PropertiesLK;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Infrastructure.PropertiesLK
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly MuniLKDbContext _context;

        public PropertyRepository(MuniLKDbContext context)
        {
            _context = context;
        }

        public async Task<Guid?> CreateAsync(Property property, CancellationToken cancellationToken = default)
        {
            _context.Properties.Add(property);
            await _context.SaveChangesAsync(cancellationToken);
            return property.Id;
        }

        public async Task<IEnumerable<Property>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return Enumerable.Empty<Property>();

            query = query.ToLower();

            return await _context.Properties
                .Where(p =>
                    (!string.IsNullOrEmpty(p.Address) && p.Address.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(p.TitleDeedNumber) && p.TitleDeedNumber.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(p.PropertyId) && p.PropertyId.ToLower().Contains(query)))
                .ToListAsync();
        }

        public IEnumerable<Property> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return Enumerable.Empty<Property>();

            query = query.ToLower();

            return _context.Properties
                .Where(p =>
                    (!string.IsNullOrEmpty(p.Address) && p.Address.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(p.TitleDeedNumber) && p.TitleDeedNumber.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(p.PropertyId) && p.PropertyId.ToLower().Contains(query)))
                .ToList();
        }

        // NEW: Get by Id
        public async Task<Property?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Properties
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }
    }
}
