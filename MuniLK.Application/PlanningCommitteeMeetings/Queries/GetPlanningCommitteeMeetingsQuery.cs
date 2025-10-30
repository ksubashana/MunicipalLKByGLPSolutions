using MediatR;
using MuniLK.Application.PlanningCommitteeMeetings.DTOs;
using MuniLK.Application.PlanningCommitteeMeetings.Interfaces;
using MuniLK.Application.Generic.Interfaces;

namespace MuniLK.Application.PlanningCommitteeMeetings.Queries
{
    public record GetPlanningCommitteeMeetingsQuery(DateTime? Start, DateTime? End, Guid? ChairpersonContactId) : IRequest<List<PlanningCommitteeMeetingResponse>>;

    public class GetPlanningCommitteeMeetingsQueryHandler : IRequestHandler<GetPlanningCommitteeMeetingsQuery, List<PlanningCommitteeMeetingResponse>>
    {
        private readonly IPlanningCommitteeMeetingRepository _repo;
        private readonly ICurrentTenantService _tenant;
        public GetPlanningCommitteeMeetingsQueryHandler(IPlanningCommitteeMeetingRepository repo, ICurrentTenantService tenant)
        { _repo = repo; _tenant = tenant; }

        public async Task<List<PlanningCommitteeMeetingResponse>> Handle(GetPlanningCommitteeMeetingsQuery request, CancellationToken cancellationToken)
        {
            var start = request.Start ?? DateTime.UtcNow.AddDays(-30);
            var end = request.End ?? DateTime.UtcNow.AddDays(60);
            var meetings = await _repo.GetRangeAsync(start, end, request.ChairpersonContactId, cancellationToken);
            return meetings.Select(m => new PlanningCommitteeMeetingResponse
            {
                Id = m.Id,
                Subject = m.Subject,
                Agenda = m.Agenda,
                StartTime = m.StartTime,
                EndTime = m.EndTime,
                Venue = m.Venue,
                ChairpersonContactId = m.ChairpersonContactId,
                Status = m.Status,
                MemberContactIds = m.Members.Where(mm => !mm.IsDeleted).Select(mm => mm.ContactId).ToList(),
                ApplicationIds = m.Applications.Where(a => !a.IsDeleted).Select(a => a.BuildingPlanApplicationId).ToList()
            }).ToList();
        }
    }
}
