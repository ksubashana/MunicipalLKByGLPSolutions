using MediatR;
using MuniLK.Application.Tenants;
using MuniLK.Application.Tenants.Commands;
using MuniLK.Domain.Entities;

public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Guid>
{
    private readonly ITenantRepository _tenantRepository;

    public CreateTenantCommandHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<Guid> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        //if (await _tenantRepository.SubdomainExistsAsync(request.Subdomain))
        //    throw new InvalidOperationException("Subdomain already in use.");

        var tenant = new Tenant
        {
            TenantId = Guid.NewGuid(),
            Name = request.Name,
            Subdomain = request.Subdomain,
            ContactEmail = request.ContactEmail,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            LastModifiedDate = DateTime.UtcNow
        };

        return await _tenantRepository.CreateAsync(tenant, cancellationToken);
    }
}
