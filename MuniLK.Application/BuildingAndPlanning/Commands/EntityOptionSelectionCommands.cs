using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.Generic.Result;
using System;
using System.Collections.Generic;

namespace MuniLK.Application.BuildingAndPlanning.Commands
{
    /// <summary>
    /// Command to save entity option selections (insert/update)
    /// </summary>
    public record SaveEntityOptionSelectionsCommand(
        Guid EntityId,
        string EntityType,
        Guid ModuleId,
        List<Guid> OptionItemIds
    ) : IRequest<Result<EntityOptionSelectionsResponse>>;

    /// <summary>
    /// Command to delete entity option selections
    /// </summary>
    public record DeleteEntityOptionSelectionsCommand(
        Guid EntityId,
        string EntityType,
        Guid ModuleId
    ) : IRequest<Result>;
}
