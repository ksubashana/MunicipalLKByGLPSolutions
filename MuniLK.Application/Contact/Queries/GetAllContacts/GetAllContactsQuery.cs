using MediatR;
using MuniLK.Application.Contact.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Contact.Queries.GetAllContacts
{
    public record GetAllContactsQuery() : IRequest<IEnumerable<ContactResponse>>;

}
