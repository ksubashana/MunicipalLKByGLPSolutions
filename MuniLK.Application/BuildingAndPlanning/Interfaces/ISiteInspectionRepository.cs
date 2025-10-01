using MuniLK.Domain.Entities;
using MuniLK.Application.BuildingAndPlanning.DTOs;

namespace MuniLK.Application.BuildingAndPlanning.Interfaces
{
    public interface ISiteInspectionRepository
    {
        Task<SiteInspection> AddAsync(SiteInspection siteInspection, CancellationToken cancellationToken = default);
        Task<SiteInspection?> GetByInspectionIdAsync(Guid inspectionId, CancellationToken cancellationToken = default);
        Task<IEnumerable<SiteInspection>> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}