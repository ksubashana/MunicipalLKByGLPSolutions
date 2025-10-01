using MuniLK.Domain.Entities;
using MuniLK.Domain.Interfaces;

namespace MuniLK.Application.BuildingAndPlanning.Interfaces
{
    public interface IPlanningCommitteeReviewRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<PlanningCommitteeReview?> GetByApplicationIdAsync(Guid applicationId, CancellationToken ct = default);
        Task AddAsync(PlanningCommitteeReview entity, CancellationToken ct = default);
        Task UpdateAsync(PlanningCommitteeReview entity, CancellationToken ct = default);
    }
}