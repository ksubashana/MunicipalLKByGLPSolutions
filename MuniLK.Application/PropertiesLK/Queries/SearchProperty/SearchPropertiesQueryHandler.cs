using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MuniLK.Application.PropertiesLK.DTOs;
using MuniLK.Application.PropertiesLK.Queries.SearchProperty;
using MuniLK.Applications.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MuniLK.Application.PropertiesLK.Mappings;

namespace MuniLK.Application.PropertiesLK.Queries.SearchProperty
{
    // Application/Properties/Handlers/SearchPropertiesQueryHandler.cs


    public class SearchPropertiesQueryHandler : IRequestHandler<SearchPropertiesQuery, IEnumerable<PropertyResponse>>
    {
        private readonly IPropertyRepository _repository;

        public SearchPropertiesQueryHandler(IPropertyRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PropertyResponse>> Handle(SearchPropertiesQuery request, CancellationToken cancellationToken)
        {
            var matched = await _repository.SearchAsync(request.Query);
            return matched.Select(p => p.ToResponse());
        }
    }

   

}
