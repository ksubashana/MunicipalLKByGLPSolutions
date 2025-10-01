using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;

namespace MuniLK.Application.BuildingAndPlanning.Queries
{
    public record SearchBuildingPlansQuery() : IRequest<List<BuildingPlanApplicationDto>>;
}