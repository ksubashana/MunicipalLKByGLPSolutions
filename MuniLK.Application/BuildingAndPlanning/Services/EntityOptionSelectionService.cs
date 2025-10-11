using MediatR;
using MuniLK.Application.BuildingAndPlanning.Commands;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.BuildingAndPlanning.Queries;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.Services
{
    /// <summary>
    /// Service implementation for managing entity option selections
    /// </summary>
    public class EntityOptionSelectionService : IEntityOptionSelectionService
    {
        private readonly IMediator _mediator;

        public EntityOptionSelectionService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> SaveSelectionsAsync(
            Guid entityId, 
            string entityType, 
            Guid moduleId, 
            List<Guid> optionItemIds, 
            CancellationToken ct = default)
        {
            var command = new SaveEntityOptionSelectionsCommand(
                entityId, 
                entityType, 
                moduleId, 
                optionItemIds);

            var result = await _mediator.Send(command, ct);
            return result.Succeeded;
        }

        public async Task<List<Guid>> GetSelectionsAsync(
            Guid entityId, 
            string entityType, 
            Guid moduleId, 
            CancellationToken ct = default)
        {
            var query = new GetEntityOptionSelectionsQuery(
                entityId, 
                entityType, 
                moduleId);

            return await _mediator.Send(query, ct);
        }
    }
}
