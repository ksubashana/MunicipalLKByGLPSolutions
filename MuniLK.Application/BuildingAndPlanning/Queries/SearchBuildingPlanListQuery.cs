using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;

namespace MuniLK.Application.BuildingAndPlanning.Queries
{
    public record SearchBuildingPlanListQuery(Guid tenantId, int Skip, int Take, string? Search)
        : IRequest<(List<BuildingPlanListItemDto> Items, int Total)>;
}