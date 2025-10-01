// MuniLK.Application/Contacts/Handlers/UpdateContactCommandHandler.cs
using MediatR;
using MuniLK.Application.Contact.Commands.UpdateContact;
using MuniLK.Application.Contacts.Commands;

using MuniLK.Applications.Interfaces;
using MuniLK.Domain.Entities; // For Contact entity
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.Contacts.Handlers
{
    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, bool>
    {
        private readonly IContactRepository _repository;

        public UpdateContactCommandHandler(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            var contactToUpdate = await _repository.GetByIdAsync(request.Id);

            if (contactToUpdate == null)
            {
                // You might throw a custom NotFoundException here
                return false;
            }

            // Map DTO fields to the existing entity
            // IMPORTANT: Create an extension method `UpdateFromDto` or similar in your Mappers
            contactToUpdate.FullName = request.Request.FullName;
            contactToUpdate.NIC = request.Request.NIC;
            contactToUpdate.Email = request.Request.Email;
            contactToUpdate.PhoneNumber = request.Request.PhoneNumber;
            contactToUpdate.AddressLine1 = request.Request.AddressLine1;
            contactToUpdate.AddressLine2 = request.Request.AddressLine2;
            contactToUpdate.City = request.Request.City;
            contactToUpdate.District = request.Request.District;
            contactToUpdate.Province = request.Request.Province;
            contactToUpdate.PostalCode = request.Request.PostalCode;
            contactToUpdate.IsActive = request.Request.IsActive;
            contactToUpdate.UpdatedAt = DateTime.UtcNow; // Set update timestamp

            await _repository.UpdateAsync(contactToUpdate);
            return true;
        }
    }
}