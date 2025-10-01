using Microsoft.EntityFrameworkCore;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Domain.Entities;
using MuniLK.Domain.Interfaces;
using MuniLK.Infrastructure.Data;

namespace MuniLK.Infrastructure.BuildingAndPlanning
{
    /// <summary>
    /// Repository implementation for Planning Committee Review operations
    /// </summary>
    public class PlanningCommitteeReviewRepository : IPlanningCommitteeReviewRepository
    {
        private readonly MuniLKDbContext _dbContext;

        public PlanningCommitteeReviewRepository(MuniLKDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUnitOfWork UnitOfWork => _dbContext;

        public async Task<PlanningCommitteeReview?> GetByApplicationIdAsync(Guid applicationId, CancellationToken ct = default)
        {
            return await _dbContext.PlanningCommitteeReviews
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ApplicationId == applicationId, ct);
        }

        public async Task AddAsync(PlanningCommitteeReview entity, CancellationToken ct = default)
        {
            await _dbContext.Set<PlanningCommitteeReview>().AddAsync(entity, ct);
        }

        public Task UpdateAsync(PlanningCommitteeReview entity, CancellationToken ct = default)
        {
            _dbContext.Set<PlanningCommitteeReview>().Update(entity);
            return Task.CompletedTask;
        }
    }
}