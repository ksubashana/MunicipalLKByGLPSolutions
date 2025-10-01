using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using System;

namespace MuniLK.Application.BuildingAndPlanning.Queries
{
    public record GetBuildingPlanSummaryQuery(Guid Id) : IRequest<BuildingPlanResponse?>;
}