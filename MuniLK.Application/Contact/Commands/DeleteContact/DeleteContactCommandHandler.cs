// MuniLK.Application/Contacts/Handlers/DeleteContactCommandHandler.cs
using MediatR;
using MuniLK.Application.Contact.Commands.DeleteContact;
using MuniLK.Applications.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.Contacts.Handlers
{
    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, bool>
    {
        private readonly IContactRepository _repository;

        public DeleteContactCommandHandler(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            var contactToDelete = await _repository.GetByIdAsync(request.Id);

            if (contactToDelete == null)
            {
                // You might throw a custom NotFoundException here
                return false;
            }

            await _repository.DeleteAsync(request.Id);
            return true;
        }
    }
}