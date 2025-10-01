using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.Generic.Interfaces;

namespace MuniLK.Application.BuildingAndPlanning.Queries
{
    public class SearchBuildingPlanListQueryHandler
        : IRequestHandler<SearchBuildingPlanListQuery, (List<BuildingPlanListItemDto> Items, int Total)>
    {
        private readonly IBuildingPlanRepository _repo;
        private readonly ICurrentTenantService _tenant;

        public SearchBuildingPlanListQueryHandler(IBuildingPlanRepository repo, ICurrentTenantService tenant)
        {
            _repo = repo;
            _tenant = tenant;
        }

        public async Task<(List<BuildingPlanListItemDto> Items, int Total)> Handle(
            SearchBuildingPlanListQuery request, CancellationToken cancellationToken)
        {
            _tenant.SetTenantId(request.tenantId);
            return await _repo.SearchListAsync(request.tenantId, request.Skip, request.Take, request.Search, cancellationToken);
        }
    }
}