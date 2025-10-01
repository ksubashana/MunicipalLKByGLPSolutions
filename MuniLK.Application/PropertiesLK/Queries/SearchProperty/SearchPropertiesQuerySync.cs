using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Application/Properties/Queries/SearchProperties/SearchPropertiesQuery.cs
using MediatR;
using MuniLK.Application.PropertiesLK.DTOs;

namespace MuniLK.Application.PropertiesLK.Queries.SearchProperty
{

    public record SearchPropertiesQuerySync(string Query) : IRequest<IEnumerable<PropertyResponse>>;

}
