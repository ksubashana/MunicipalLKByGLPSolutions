using MediatR;
using MuniLK.Application.Generic.Result;
using MuniLK.Application.PlanningCommitteeMeetings.DTOs;
using MuniLK.Application.PlanningCommitteeMeetings.Interfaces;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.ScheduleAppointment.Interfaces;
using MuniLK.Domain.Entities;
using MuniLK.Domain.Constants.Flows;

namespace MuniLK.Application.PlanningCommitteeMeetings.Commands
{
    public record CreatePlanningCommitteeMeetingCommand(PlanningCommitteeMeetingRequest Request, string UserId) : IRequest<Result<PlanningCommitteeMeetingResponse>>;

    public class CreatePlanningCommitteeMeetingCommandHandler : IRequestHandler<CreatePlanningCommitteeMeetingCommand, Result<PlanningCommitteeMeetingResponse>>
    {
        private readonly IPlanningCommitteeMeetingRepository _repo;
        private readonly ICurrentTenantService _tenant;
        private readonly IBuildingPlanRepository _bpRepo;
        private readonly IWorkflowService _workflow;
        private readonly IScheduleAppointmentRepository _scheduleRepo;

        public CreatePlanningCommitteeMeetingCommandHandler(IPlanningCommitteeMeetingRepository repo, ICurrentTenantService tenant, IBuildingPlanRepository bpRepo, IWorkflowService workflow, IScheduleAppointmentRepository scheduleRepo)
        {
            _repo = repo; _tenant = tenant; _bpRepo = bpRepo; _workflow = workflow; _scheduleRepo = scheduleRepo;
        }

        public async Task<Result<PlanningCommitteeMeetingResponse>> Handle(CreatePlanningCommitteeMeetingCommand request, CancellationToken cancellationToken)
        {
            var r = request.Request;
            if (r.StartTime >= r.EndTime) return Result<PlanningCommitteeMeetingResponse>.Failure("Start time must be before end time");
            var tenantId = _tenant.GetTenantId();
            var overlap = await _repo.ExistsOverlapAsync(r.StartTime, r.EndTime, tenantId, null, cancellationToken);
            if (overlap) return Result<PlanningCommitteeMeetingResponse>.Failure("Meeting times overlap with existing meeting");
            if (r.MemberContactIds.Distinct().Count() < 2) return Result<PlanningCommitteeMeetingResponse>.Failure("At least two members required");
            if (r.MemberContactIds.Contains(r.ChairpersonContactId)) return Result<PlanningCommitteeMeetingResponse>.Failure("Chairperson cannot be in members list (will be added automatically)");

            var meeting = new PlanningCommitteeMeeting
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                Subject = r.Subject,
                Agenda = r.Agenda,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                Venue = r.Venue,
                ChairpersonContactId = r.ChairpersonContactId,
                Status = PlanningCommitteeMeetingStatus.Scheduled,
                CreatedBy = request.UserId,
                CreatedOn = DateTime.UtcNow
            };
            await _repo.AddAsync(meeting, cancellationToken);

            // Add members (include chairperson as IsChair)
            await _repo.AddMemberAsync(new PlanningCommitteeMeetingMember { Id = Guid.NewGuid(), MeetingId = meeting.Id, ContactId = r.ChairpersonContactId, IsChair = true }, cancellationToken);
            foreach (var m in r.MemberContactIds.Distinct())
            {
                await _repo.AddMemberAsync(new PlanningCommitteeMeetingMember { Id = Guid.NewGuid(), MeetingId = meeting.Id, ContactId = m, IsChair = false }, cancellationToken);
            }

            // Link applications & add workflow logs
            foreach (var appId in r.ApplicationIds.Distinct())
            {
                await _repo.AddApplicationAsync(new PlanningCommitteeMeetingApplication { Id = Guid.NewGuid(), MeetingId = meeting.Id, BuildingPlanApplicationId = appId }, cancellationToken);
                var app = await _bpRepo.GetForUpdateAsync(appId, cancellationToken);
                if (app != null)
                {
                    var prev = app.Status.ToString();
                    if (app.Status == BuildingAndPlanSteps.ToReview) app.Status = BuildingAndPlanSteps.AssignToCommittee;
                    await _workflow.AddLogAsync(app.Id, "Committee Meeting Scheduled", prev, app.Status.ToString(), r.Agenda, request.UserId, "Officer", null, false, cancellationToken);
                }
            }

            // Create ScheduleAppointments for each member (including chairperson)
            var allParticipants = r.MemberContactIds.Append(r.ChairpersonContactId).Distinct();
            foreach (var cid in allParticipants)
            {
                var sched = new ScheduleAppointments
                {
                    Subject = meeting.Subject,
                    Description = (meeting.Agenda ?? string.Empty).Length > 380 ? meeting.Agenda!.Substring(0, 380) + "..." : meeting.Agenda ?? string.Empty,
                    StartTime = meeting.StartTime,
                    EndTime = meeting.EndTime,
                    Location = meeting.Venue,
                    OwnerId = cid,
                    OwnerRole = cid == meeting.ChairpersonContactId ? "Chairperson" : "Member",
                    AppointmentGroup = "PlanningCommittee",
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.UtcNow,
                    CustomStyle = "pc-meeting-scheduled"
                };
                await _scheduleRepo.AddAsync(sched);
            }

            await _repo.SaveChangesAsync(cancellationToken);
            // Save building plan changes
            await _bpRepo.UnitOfWork.SaveChangesAsync(cancellationToken);

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
                MemberContactIds = allParticipants.ToList(),
                ApplicationIds = r.ApplicationIds.Distinct().ToList()
            };
            return Result<PlanningCommitteeMeetingResponse>.Success(resp);
        }
    }
}
