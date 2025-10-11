using MediatR;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.BuildingAndPlanning.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.Handlers
{
    public class GetEntityOptionSelectionsQueryHandler 
        : IRequestHandler<GetEntityOptionSelectionsQuery, List<Guid>>
    {
        private readonly IEntityOptionSelectionRepository _repository;

        public GetEntityOptionSelectionsQueryHandler(IEntityOptionSelectionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Guid>> Handle(
            GetEntityOptionSelectionsQuery request, 
            CancellationToken ct)
        {
            var selections = await _repository.GetSelectionsAsync(
                request.EntityId, 
                request.EntityType, 
                request.ModuleId, 
                ct);

            return selections.Select(s => s.OptionItemId).ToList();
        }
    }
}
