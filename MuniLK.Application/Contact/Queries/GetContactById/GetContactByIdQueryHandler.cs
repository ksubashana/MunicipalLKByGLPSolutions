// MuniLK.Application/Contacts/Handlers/GetContactByIdQueryHandler.cs
using MediatR;
using MuniLK.Application.Contact.DTOs;
using MuniLK.Application.Contact.Queries.GetContactById;

using MuniLK.Applications.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.Contacts.Handlers
{
    public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, ContactResponse>
    {
        private readonly IContactRepository _repository;

        public GetContactByIdQueryHandler(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<ContactResponse> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
        {
            var contact = await _repository.GetByIdAsync(request.Id);

            // Map entity to response DTO
            return contact?.ToResponse(); // Use the extension method
        }
    }
}