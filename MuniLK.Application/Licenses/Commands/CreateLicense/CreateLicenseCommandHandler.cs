using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.LicensesLK.Mappings;
using MuniLK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

public class CreateLicenseCommandHandler : IRequestHandler<CreateLicenseCommand, Guid?>
{
    private readonly ILicenseRepository _repository;
    private readonly ILogger _logger;
    private readonly ICurrentTenantService _currentTenantService;

    public CreateLicenseCommandHandler(ILicenseRepository repository,ICurrentTenantService currentTenantService)
    {
        _repository = repository;
        _currentTenantService = currentTenantService;
    }

    public async Task<Guid?> Handle(CreateLicenseCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = _currentTenantService.GetTenantId();
        License license = request.Request.ToEntity(tenantId);
        try
        {
            
            if (license == null)
            {
                _logger.LogError("License data cannot be null.");
                throw new InvalidLicensePropertyException("License", "License data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(license.LicenseNumber))
            {
                _logger.LogError("License name cannot be empty.");
                throw new InvalidLicensePropertyException("Name", "License name cannot be empty.");
            }

            if (license.ExpiryDate <= DateTime.UtcNow)
            {
                _logger.LogError("License expiry date must be in the future: {ExpiryDate}", license.ExpiryDate);
                throw new InvalidLicensePropertyException("ExpiryDate", "Expiry date must be in the future.");
            }
            return await _repository.CreateAsync(license, cancellationToken);

        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating license {LicenseName}.", license.LicenseNumber);
            throw new LicenseOperationException("An error occurred while creating the license.");
        }
    }
}