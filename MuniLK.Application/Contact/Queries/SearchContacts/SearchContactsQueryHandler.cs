// MuniLK.Application/Contacts/Handlers/SearchContactsQueryHandler.cs
using MediatR;
using MuniLK.Application.Contact.DTOs;
using MuniLK.Application.Contacts.Queries.SearchContacts;
using MuniLK.Applications.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.Contacts.Handlers
{
    public class SearchContactsQueryHandler : IRequestHandler<SearchContactsQuery, IEnumerable<ContactResponse>>
    {
        private readonly IContactRepository _repository;

        public SearchContactsQueryHandler(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ContactResponse>> Handle(SearchContactsQuery request, CancellationToken cancellationToken)
        {
            var contacts = await _repository.SearchAsync(request.Query);
            return contacts.Select(c => c.ToResponse());
        }

    }
}
