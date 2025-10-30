using MediatR;
using MuniLK.Application.Generic.Result;
using MuniLK.Application.PlanningCommitteeMeetings.DTOs;
using MuniLK.Application.PlanningCommitteeMeetings.Interfaces;
using MuniLK.Application.ScheduleAppointment.Interfaces;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Domain.Constants.Flows;

namespace MuniLK.Application.PlanningCommitteeMeetings.Commands
{
    public record UpdatePlanningCommitteeMeetingCommand(Guid MeetingId, PlanningCommitteeMeetingRequest Request, string UserId) : IRequest<Result<PlanningCommitteeMeetingResponse>>;

    public class UpdatePlanningCommitteeMeetingCommandHandler : IRequestHandler<UpdatePlanningCommitteeMeetingCommand, Result<PlanningCommitteeMeetingResponse>>
    {
        private readonly IPlanningCommitteeMeetingRepository _repo;
        private readonly IScheduleAppointmentRepository _scheduleRepo;
        private readonly ICurrentTenantService _tenant;

        public UpdatePlanningCommitteeMeetingCommandHandler(IPlanningCommitteeMeetingRepository repo, IScheduleAppointmentRepository scheduleRepo, ICurrentTenantService tenant)
        { _repo = repo; _scheduleRepo = scheduleRepo; _tenant = tenant; }

        public async Task<Result<PlanningCommitteeMeetingResponse>> Handle(UpdatePlanningCommitteeMeetingCommand request, CancellationToken cancellationToken)
        {
            var meeting = await _repo.GetByIdAsync(request.MeetingId, cancellationToken);
            if (meeting == null || meeting.IsDeleted) return Result<PlanningCommitteeMeetingResponse>.Failure("Meeting not found");
            var r = request.Request;
            if (r.StartTime >= r.EndTime) return Result<PlanningCommitteeMeetingResponse>.Failure("Start time must be before end time");
            var overlap = await _repo.ExistsOverlapAsync(r.StartTime, r.EndTime, _tenant.GetTenantId(), meeting.Id, cancellationToken);
            if (overlap) return Result<PlanningCommitteeMeetingResponse>.Failure("New times overlap with another meeting");

            meeting.Subject = r.Subject;
            meeting.Agenda = r.Agenda;
            meeting.StartTime = r.StartTime;
            meeting.EndTime = r.EndTime;
            meeting.Venue = r.Venue;
            meeting.UpdatedBy = request.UserId;
            meeting.UpdatedOn = DateTime.UtcNow;

            // Update schedule appointments for participants in this meeting group
            var allSched = await _scheduleRepo.GetByDateRangeAsync(meeting.StartTime.AddDays(-30), meeting.EndTime.AddDays(30));
            var related = allSched.Where(a => a.AppointmentGroup == "PlanningCommittee" && a.Subject == meeting.Subject && a.StartTime == meeting.StartTime && a.EndTime == meeting.EndTime); // simplistic match
            foreach (var ap in related)
            {
                ap.StartTime = meeting.StartTime;
                ap.EndTime = meeting.EndTime;
                ap.Location = meeting.Venue;
                ap.Description = (meeting.Agenda ?? string.Empty).Length > 380 ? meeting.Agenda!.Substring(0, 380) + "..." : meeting.Agenda ?? string.Empty;
                ap.UpdatedBy = request.UserId;
                ap.UpdatedDate = DateTime.UtcNow;
                await _scheduleRepo.UpdateAsync(ap);
            }

            await _repo.UpdateAsync(meeting, cancellationToken);
            await _repo.SaveChangesAsync(cancellationToken);

            var resp = new PlanningCommitteeMeetingResponse
            {
                Id = meeting.Id,
                Subject = meeting.Subject,
                Agenda = meeting.Agenda,
                StartTime = meeting.StartTime,
                EndTime = meeting.EndTime,
                Venue = meeting.Venue,
                ChairpersonContactId = meeting.ChairpersonContactId,
                Status = meeting.Status,
                MemberContactIds = meeting.Members.Select(m => m.ContactId).ToList(),
                ApplicationIds = meeting.Applications.Select(a => a.BuildingPlanApplicationId).ToList()
            };
            return Result<PlanningCommitteeMeetingResponse>.Success(resp);
        }
    }
}
