// MuniLK.Application/Contacts/Handlers/GetAllContactsQueryHandler.cs
using MediatR;
using MuniLK.Application.Contact.DTOs;
using MuniLK.Application.Contact.Queries.GetAllContacts;

using MuniLK.Applications.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.Contacts.Handlers
{
    public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, IEnumerable<ContactResponse>>
    {
        private readonly IContactRepository _repository;

        public GetAllContactsQueryHandler(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ContactResponse>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
        {
            var contacts = await _repository.GetAllAsync();

            // Map entities to response DTOs
            return contacts.Select(c => c.ToResponse()).ToList(); // Use the extension method
        }
    }
}