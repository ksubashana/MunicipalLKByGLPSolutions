using MediatR;
using MuniLK.Application.Contact.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Contact.Queries.GetContactById
{
    public record GetContactByIdQuery(Guid Id) : IRequest<ContactResponse>;

}
