using Microsoft.EntityFrameworkCore;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
public class LicenseRepository : ILicenseRepository
{
    private readonly MuniLKDbContext _context;

    public LicenseRepository(MuniLKDbContext context)
    {
        _context = context;
    }

    public async Task<License?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Licenses.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<Guid?> CreateAsync(License license, CancellationToken cancellationToken = default)
    {
        _context.Licenses.Add(license);
        await _context.SaveChangesAsync(cancellationToken);
        return license.Id;
    }

    public async Task<IEnumerable<License>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Licenses.ToListAsync(cancellationToken);
    }
}