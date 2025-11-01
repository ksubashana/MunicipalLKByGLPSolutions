using Microsoft.EntityFrameworkCore;
using MuniLK.Application.Tenants;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;

public class TenantRepository : ITenantRepository
{
    private readonly  MuniLKDbContext _context;

    public TenantRepository(MuniLKDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateAsync(Tenant tenant, CancellationToken cancellationToken)
    {
        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync(cancellationToken);
        return tenant.TenantId;
    }

    public async Task<bool> SubdomainExistsAsync(string subdomain)
    {
        return await _context.Tenants.AnyAsync(t => t.Subdomain == subdomain, cancellationToken: CancellationToken.None);
    }

    public async Task<List<Tenant>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Tenants.OrderBy(t => t.Name).ToListAsync(cancellationToken);
    }
}
