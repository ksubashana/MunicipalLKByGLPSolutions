using Domain.Exceptions;
using MediatR;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.PropertyOwners.DTOs;
using MuniLK.Application.PropertyOwners.Interfaces;
using MuniLK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.PropertyOwners.Commands.CreatePropertyOwner
{
    public class CreatePropertyOwnerCommandHandler
        : IRequestHandler<CreatePropertyOwnerCommand, PropertyOwnerResponse>
    {
        private readonly IPropertyOwnerRepository _repository;
        private readonly ICurrentTenantService _currentTenantService;

        public CreatePropertyOwnerCommandHandler(
            IPropertyOwnerRepository repository,
            ICurrentTenantService currentTenantService)
        {
            _repository = repository;
            _currentTenantService = currentTenantService;
        }

        public async Task<PropertyOwnerResponse> Handle(CreatePropertyOwnerCommand command, CancellationToken cancellationToken)
        {
            var exists = await _repository.ExistsAsync(command.Request.PropertyId, command.Request.ContactId, _currentTenantService.GetTenantId(),cancellationToken);

            if (exists)
                throw new PropertyOwnerAlreadyExistsException();

            var entity = new PropertyOwner
            {
                Id = Guid.NewGuid(),
                PropertyId = command.Request.PropertyId,
                ContactId = command.Request.ContactId,
                OwnershipType = command.Request.OwnershipType,
                TenantId = _currentTenantService.GetTenantId(),
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "system"
            };

            await _repository.AddAsync(entity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return new PropertyOwnerResponse
            {
                Id = entity.Id,
                PropertyId = entity.PropertyId!.Value,
                ContactId = entity.ContactId,
                OwnershipType = entity.OwnershipType
            };
        }
    }
}
