using MuniLK.Domain.Entities;
using MuniLK.Application.BuildingAndPlanning.DTOs;

namespace MuniLK.Application.BuildingAndPlanning.Interfaces
{
    public interface ISiteInspectionRepository
    {
        Task<SiteInspection> AddAsync(SiteInspection siteInspection, CancellationToken cancellationToken = default);
        Task<SiteInspection?> GetByInspectionIdAsync(Guid inspectionId, CancellationToken cancellationToken = default);
        Task<IEnumerable<SiteInspection>> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Upsert by business key InspectionId. If a record with the same InspectionId exists it is updated; otherwise a new one is added.
        /// Returns the tracked entity (new or updated) and a flag indicating whether it was created.
        /// </summary>
        Task<(SiteInspection Entity, bool Created)> AddOrUpdateByInspectionIdAsync(SiteInspection siteInspection, CancellationToken cancellationToken = default);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}