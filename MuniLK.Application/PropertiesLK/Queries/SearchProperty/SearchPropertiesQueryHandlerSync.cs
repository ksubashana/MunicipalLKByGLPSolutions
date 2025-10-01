using MediatR;
using MuniLK.Application.PropertiesLK.DTOs;
using MuniLK.Application.PropertiesLK.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.PropertiesLK.Queries.SearchProperty
{
    public class SearchPropertiesQueryHandlerSync : IRequestHandler<SearchPropertiesQuerySync, IEnumerable<PropertyResponse>>
    {
        private readonly IPropertyRepository _repository;

        public SearchPropertiesQueryHandlerSync(IPropertyRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<PropertyResponse>> Handle(SearchPropertiesQuerySync request, CancellationToken cancellationToken)
        {
            var matched = _repository.Search(request.Query); // Sync search method
            var result = matched.Select(p => p.ToResponse()).ToList(); // Materialize and map
            return Task.FromResult<IEnumerable<PropertyResponse>>(result); // Wrap in Task
        }
    }
}