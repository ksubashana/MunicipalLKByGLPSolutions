// MuniLK.Application/Contacts/Queries/SearchContacts/SearchContactsQuery.cs
using MediatR;
using MuniLK.Application.Contact.DTOs;
using System.Collections.Generic;

namespace MuniLK.Application.Contacts.Queries.SearchContacts
{
    public record SearchContactsQuery(string Query) : IRequest<IEnumerable<ContactResponse>>;
}
