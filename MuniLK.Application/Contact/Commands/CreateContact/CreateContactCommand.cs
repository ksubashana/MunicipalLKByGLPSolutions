using MediatR;
using MuniLK.Application.Contact.DTOs;
using MuniLK.Application.Generic.Result;
using System;

namespace MuniLK.Application.Contacts.Commands
{
    public record CreateContactCommand(CreateContactRequest Request) : IRequest<Result<ContactResponse>>;

}
