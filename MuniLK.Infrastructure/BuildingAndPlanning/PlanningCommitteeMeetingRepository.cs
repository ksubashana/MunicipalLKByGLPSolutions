using Microsoft.EntityFrameworkCore;
using MuniLK.Application.PlanningCommitteeMeetings.Interfaces;
using MuniLK.Domain.Entities;
using MuniLK.Domain.Interfaces;
using MuniLK.Infrastructure.Data;

namespace MuniLK.Infrastructure.BuildingAndPlanning
{
    public class PlanningCommitteeMeetingRepository : IPlanningCommitteeMeetingRepository
    {
        private readonly MuniLKDbContext _db;
        public PlanningCommitteeMeetingRepository(MuniLKDbContext db) => _db = db;
        public IUnitOfWork UnitOfWork => _db;

        public async Task<PlanningCommitteeMeeting?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _db.Set<PlanningCommitteeMeeting>()
                .Include(m => m.Members)
                .Include(m => m.Applications)
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted, ct);

        public async Task<List<PlanningCommitteeMeeting>> GetRangeAsync(DateTime start, DateTime end, CancellationToken ct = default)
        {
            // Ignore global query filters (e.g., tenant filter) as requested
            var q = _db.Set<PlanningCommitteeMeeting>()
                .IgnoreQueryFilters()
                .Where(m => !m.IsDeleted && m.StartTime >= start && m.EndTime <= end)
                .Include(m => m.Members)
                .Include(m => m.Applications);
            return await q.ToListAsync(ct);
        }

        public async Task<bool> ExistsOverlapAsync(DateTime start, DateTime end, Guid? tenantId, Guid? excludeMeetingId = null, CancellationToken ct = default)
        {
            return await _db.Set<PlanningCommitteeMeeting>().AnyAsync(m => !m.IsDeleted && m.TenantId == tenantId && (excludeMeetingId == null || m.Id != excludeMeetingId) && (
                (start >= m.StartTime && start < m.EndTime) ||
                (end > m.StartTime && end <= m.EndTime) ||
                (start <= m.StartTime && end >= m.EndTime)), ct);
        }

        public async Task AddAsync(PlanningCommitteeMeeting meeting, CancellationToken ct = default)
            => await _db.AddAsync(meeting, ct);

        public Task UpdateAsync(PlanningCommitteeMeeting meeting, CancellationToken ct = default)
        {
            _db.Update(meeting);
            return Task.CompletedTask;
        }

        public async Task SoftDeleteAsync(Guid id, string userId, CancellationToken ct = default)
        {
            var entity = await _db.Set<PlanningCommitteeMeeting>().FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.UpdatedBy = userId;
                entity.UpdatedOn = DateTime.UtcNow;
                _db.Update(entity);
            }
        }

        public async Task AddMemberAsync(PlanningCommitteeMeetingMember member, CancellationToken ct = default)
            => await _db.AddAsync(member, ct);

        public async Task RemoveMemberAsync(Guid meetingId, Guid contactId, CancellationToken ct = default)
        {
            var m = await _db.Set<PlanningCommitteeMeetingMember>().FirstOrDefaultAsync(x => x.PlanningCommitteeMeetingId == meetingId && x.ContactId == contactId, ct);
            if (m != null)
            {
                m.IsDeleted = true;
                _db.Update(m);
            }
        }

        public async Task AddApplicationAsync(PlanningCommitteeMeetingApplication appLink, CancellationToken ct = default)
            => await _db.AddAsync(appLink, ct);

        public async Task RemoveApplicationAsync(Guid meetingId, Guid applicationId, CancellationToken ct = default)
        {
            var a = await _db.Set<PlanningCommitteeMeetingApplication>().FirstOrDefaultAsync(x => x.PlanningCommitteeMeetingId == meetingId && x.BuildingPlanApplicationId == applicationId, ct);
            if (a != null)
            {
                a.IsDeleted = true;
                _db.Update(a);
            }
        }

        public async Task SaveChangesAsync(CancellationToken ct = default) => await _db.SaveChangesAsync(ct);
    }
}
