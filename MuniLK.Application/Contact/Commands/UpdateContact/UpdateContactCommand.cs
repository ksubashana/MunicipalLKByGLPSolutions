using MediatR;
using MuniLK.Application.Contact.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Contact.Commands.UpdateContact
{
    public record UpdateContactCommand(Guid Id, UpdateContactRequest Request) : IRequest<bool>;

}
