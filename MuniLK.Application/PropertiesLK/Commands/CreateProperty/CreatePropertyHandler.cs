
using MediatR;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.PropertiesLK;
using MuniLK.Application.PropertiesLK.DTOs;
using MuniLK.Application.PropertiesLK.Mappings;

public class CreatePropertyHandler : IRequestHandler<CreatePropertyCommand, PropertyResponse>
{
    private readonly IPropertyRepository _repository;
    private readonly ICurrentTenantService _currentTenantService;
    private readonly ICurrentUserService _currentUser;

    public CreatePropertyHandler(IPropertyRepository repository, ICurrentTenantService currentTenantService, ICurrentUserService currentUser)
    {
        _repository = repository;
        _currentTenantService = currentTenantService;
        _currentUser = currentUser;
    }

    public async Task<PropertyResponse> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        CreatePropertyRequest dto = request.Request;
            Guid? tenantId = _currentTenantService.GetTenantId();

        Property property = request.Request.ToEntity( _currentUser.UserName,tenantId);

        await _repository.CreateAsync(property, cancellationToken);
        return property.ToResponse();   
    }
}
