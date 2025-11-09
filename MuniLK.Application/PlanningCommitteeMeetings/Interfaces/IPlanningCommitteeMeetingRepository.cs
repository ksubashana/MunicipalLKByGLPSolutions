using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MuniLK.Domain.Entities;
using MuniLK.Domain.Interfaces;

namespace MuniLK.Application.PlanningCommitteeMeetings.Interfaces
{
    public interface IPlanningCommitteeMeetingRepository
    {
        IUnitOfWork UnitOfWork { get; }
        Task<PlanningCommitteeMeeting?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<List<PlanningCommitteeMeeting>> GetRangeAsync(DateTime start, DateTime end, CancellationToken ct = default);
        Task<bool> ExistsOverlapAsync(DateTime start, DateTime end, Guid? tenantId, Guid? excludeMeetingId = null, CancellationToken ct = default);
        Task AddAsync(PlanningCommitteeMeeting meeting, CancellationToken ct = default);
        Task UpdateAsync(PlanningCommitteeMeeting meeting, CancellationToken ct = default);
        Task SoftDeleteAsync(Guid id, string userId, CancellationToken ct = default);
        Task AddMemberAsync(PlanningCommitteeMeetingMember member, CancellationToken ct = default);
        Task RemoveMemberAsync(Guid meetingId, Guid contactId, CancellationToken ct = default);
        Task AddApplicationAsync(PlanningCommitteeMeetingApplication appLink, CancellationToken ct = default);
        Task RemoveApplicationAsync(Guid meetingId, Guid applicationId, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
