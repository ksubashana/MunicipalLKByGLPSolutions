using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Contact.Commands.DeleteContact
{
    public record DeleteContactCommand(Guid Id) : IRequest<bool>;

}
