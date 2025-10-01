using MuniLK.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface ILicenseRepository
{
    Task<License?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid?> CreateAsync(License license, CancellationToken cancellationToken = default);
    Task<IEnumerable<License>> GetAllAsync(CancellationToken cancellationToken = default);
}