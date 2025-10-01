using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.Generic.Interfaces;

namespace MuniLK.Application.BuildingAndPlanning.Queries
{
    public class SearchBuildingPlansQueryHandler : IRequestHandler<SearchBuildingPlansQuery, List<BuildingPlanApplicationDto>>
    {
        private readonly IBuildingPlanRepository _repository;
        private readonly ICurrentTenantService _currentTenantService;

        public SearchBuildingPlansQueryHandler(IBuildingPlanRepository repository, ICurrentTenantService currentTenantService)
        {
            _repository = repository;
            _currentTenantService = currentTenantService;
        }

        // FIX: MediatR IRequestHandler requires Task<TResponse>. Previous version returned List<T>.
        public Task<List<BuildingPlanApplicationDto>> Handle(SearchBuildingPlansQuery request, CancellationToken cancellationToken)
        {
            var tenantId = _currentTenantService.GetTenantId();
            var results = _repository.SearchAsync(tenantId, cancellationToken);
            return Task.FromResult(results);
        }
    }
}