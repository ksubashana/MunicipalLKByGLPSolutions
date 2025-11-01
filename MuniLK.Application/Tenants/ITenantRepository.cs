using MuniLK.Domain.Entities;

namespace MuniLK.Application.Tenants
{
    public interface ITenantRepository
    {
        Task<Guid> CreateAsync(Tenant tenant, CancellationToken cancellationToken);
        //Task<Tenant> GetByIdAsync(Guid id);
        Task<bool> SubdomainExistsAsync(string subdomain);
        Task<List<Tenant>> GetAllAsync(CancellationToken cancellationToken); // Added for listing tenants
    }
}
