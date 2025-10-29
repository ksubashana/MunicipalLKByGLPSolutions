using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using System;

namespace MuniLK.Application.BuildingAndPlanning.Queries
{
    public record GetBuildingPlanWorkflowSnapshotQuery(Guid ApplicationId) : IRequest<BuildingPlanWorkflowSnapshot?>;
}
