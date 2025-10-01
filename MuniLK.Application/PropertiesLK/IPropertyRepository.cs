using MuniLK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.PropertiesLK
{
    public interface IPropertyRepository
    {
        Task<Guid?> CreateAsync(Property property, CancellationToken cancellationToken = default);
        Task<IEnumerable<Property>> SearchAsync(string query);
        IEnumerable<Property> Search(string query);

        // NEW: Get by Id
        Task<Property?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
