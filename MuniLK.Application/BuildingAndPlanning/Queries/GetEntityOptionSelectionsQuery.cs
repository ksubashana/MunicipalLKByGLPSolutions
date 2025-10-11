using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using System;
using System.Collections.Generic;

namespace MuniLK.Application.BuildingAndPlanning.Queries
{
    /// <summary>
    /// Query to get entity option selections
    /// </summary>
    public record GetEntityOptionSelectionsQuery(
        Guid EntityId,
        string EntityType,
        Guid ModuleId
    ) : IRequest<List<Guid>>;
}
