using MediatR;
using MuniLK.Application.PropertyOwners.DTOs;
using MuniLK.Application.PropertyOwners.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.PropertyOwners.Queries.GetOwners
{
    public class GetOwnersByPropertyIdQueryHandler : IRequestHandler<GetOwnersByPropertyIdQuery, IEnumerable<PropertyOwnerResponse>>
    {
        private readonly IPropertyOwnerRepository _propertyOwnerRepository;

        public GetOwnersByPropertyIdQueryHandler(IPropertyOwnerRepository propertyOwnerRepository)
        {
            _propertyOwnerRepository = propertyOwnerRepository;
        }

        public async Task<IEnumerable<PropertyOwnerResponse>> Handle(GetOwnersByPropertyIdQuery request, CancellationToken cancellationToken)
        {
            var owners = await _propertyOwnerRepository.GetByPropertyIdAsync(request.PropertyId, cancellationToken);

            return owners.Select(o => new PropertyOwnerResponse
            {
                Id = o.Id,
                PropertyId = o.PropertyId!.Value,
                ContactId = o.ContactId,
                OwnershipType = o.OwnershipType,
                FullName = o.Contact.FullName,
                NIC = o.Contact.NIC,
                Email = o.Contact.Email,
                Phone = o.Contact.PhoneNumber,
                Address = $"{o.Contact.AddressLine1} {o.Contact.AddressLine2}, {o.Contact.City}, {o.Contact.District}, {o.Contact.Province}, {o.Contact.PostalCode}",
                CreatedDate = o.CreatedDate,
                CreatedBy = o.CreatedBy
            });
        }
    }
}
    
