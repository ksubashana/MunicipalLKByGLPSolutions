using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Domain.Entities;
using MuniLK.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.Interfaces
{
    public interface IBuildingPlanRepository 
    {
        IUnitOfWork UnitOfWork { get; }
        
        Task AddAsync(BuildingPlanApplication entity, CancellationToken ct = default);

        // Get building plan application by id with related entities
        Task<BuildingPlanApplication?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<BuildingPlanApplication?> GetForUpdateAsync(Guid id, CancellationToken ct = default);
        
        // Get building plan application with workflow logs
        Task<BuildingPlanApplication?> GetByIdWithWorkflowLogsAsync(Guid id, CancellationToken ct = default);
        
        // Get workflow logs for an application
        Task<List<WorkflowLog>> GetWorkflowLogsAsync(Guid applicationId, CancellationToken ct = default);
        
        // Existing full load (keep if needed elsewhere)
        List<BuildingPlanApplicationDto> SearchAsync(Guid? tenantId, CancellationToken ct = default);

        // New lightweight paged list for grids
        Task<(List<BuildingPlanListItemDto> Items, int Total)> SearchListAsync(
            Guid? tenantId,
            int skip,
            int take,
            string? search,
            CancellationToken ct = default);
    }
}
