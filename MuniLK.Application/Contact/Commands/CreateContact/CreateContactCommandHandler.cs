using MediatR;
using MuniLK.Application.Contact.DTOs;
using MuniLK.Application.Contacts.Commands;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.Generic.Result;
using MuniLK.Applications.Interfaces;
using MuniLK.Domain.Interfaces;

public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, Result<ContactResponse>>
{
    private readonly IContactRepository _repository;
    private readonly ICurrentTenantService _currentTenantService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateContactCommandHandler(IContactRepository repository, ICurrentTenantService currentTenantService ,IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _currentTenantService = currentTenantService;
        _unitOfWork = unitOfWork;
    }

    //public async Task<Guid?> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    //{
    //    Guid? tenantId = _currentTenantService.GetTenantId();
    //    Contact contact = request.Request.ToEntity(tenantId);

    //    await _repository.AddAsync(contact);
    //    await _unitOfWork.SaveChangesAsync();
    //    return contact.Id;
    //}

    public async Task<Result<ContactResponse>> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = _currentTenantService.GetTenantId();

        // Use the search method to see if contact already exists (by phone, NIC, etc.)
        var existingContact = await _repository.GetByUniqueFieldsAsync(
            request.Request.NIC,
            request.Request.Email,
            request.Request.PhoneNumber,
            tenantId,
            cancellationToken
        ); 

        if (existingContact != null)
        {
            // Map to DTO to send to UI
            var existingDto = new ContactResponse
            {
                Id = existingContact.Id,
                FullName = existingContact.FullName,
                NationalId = existingContact.NIC,
                PhoneNumber = existingContact.PhoneNumber,
                Email = existingContact.Email,
                Address = existingContact.AddressLine1
            };

            return Result<ContactResponse>.Failure(
        "Contact already exists with the same NIC, Email, or Phone Number",
                existingDto
            );
        }

        // Otherwise, create new contact
        MuniLK.Domain.Entities.ContactEntities.Contact contact = request.Request.ToEntity(tenantId);
        await _repository.AddAsync(contact);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var newContactDto = new ContactResponse
        {
            Id = contact.Id,
            FullName = contact.FullName,
            NationalId = contact.NIC,
            PhoneNumber = contact.PhoneNumber,
            Email = contact.Email,
            Address = contact.AddressLine1
        };

        return Result<ContactResponse>.Success(newContactDto);
    }
}
