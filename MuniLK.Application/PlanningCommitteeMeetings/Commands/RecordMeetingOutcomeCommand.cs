using MediatR;
using MuniLK.Application.Generic.Result;
using MuniLK.Application.PlanningCommitteeMeetings.Interfaces;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Domain.Constants.Flows;
using MuniLK.Domain.Entities;

namespace MuniLK.Application.PlanningCommitteeMeetings.Commands
{
    public record RecordMeetingOutcomeCommand(Guid MeetingId, CommitteeDecision Decision, string? Notes, string UserId) : IRequest<Result>;

    public class RecordMeetingOutcomeCommandHandler : IRequestHandler<RecordMeetingOutcomeCommand, Result>
    {
        private readonly IPlanningCommitteeMeetingRepository _repo;
        private readonly IBuildingPlanRepository _bpRepo;
        private readonly IWorkflowService _workflow;

        public RecordMeetingOutcomeCommandHandler(IPlanningCommitteeMeetingRepository repo, IBuildingPlanRepository bpRepo, IWorkflowService workflow)
        { _repo = repo; _bpRepo = bpRepo; _workflow = workflow; }

        public async Task<Result> Handle(RecordMeetingOutcomeCommand request, CancellationToken cancellationToken)
        {
            var meeting = await _repo.GetByIdAsync(request.MeetingId, cancellationToken);
            if (meeting == null || meeting.IsDeleted) return Result.Failure("Meeting not found");
            if (meeting.Status != PlanningCommitteeMeetingStatus.Scheduled) return Result.Failure("Outcome already recorded or meeting invalid state");

            meeting.Status = PlanningCommitteeMeetingStatus.Completed;
            meeting.UpdatedBy = request.UserId;
            meeting.UpdatedOn = DateTime.UtcNow;
            await _repo.UpdateAsync(meeting, cancellationToken);

            foreach (var link in meeting.Applications.Where(a => !a.IsDeleted))
            {
                var app = await _bpRepo.GetForUpdateAsync(link.BuildingPlanApplicationId, cancellationToken);
                if (app == null) continue;
                var prev = app.Status.ToString();
                if (request.Decision == CommitteeDecision.Approve || request.Decision == CommitteeDecision.ApproveWithConditions)
                    app.Status = BuildingAndPlanSteps.CommissionerApproval;
                else if (request.Decision == CommitteeDecision.Reject)
                    app.Status = BuildingAndPlanSteps.Rejected;
                else if (request.Decision == CommitteeDecision.DeferForClarifications)
                    app.Status = BuildingAndPlanSteps.PlanningCommitteeReview; // remain / rework
                await _workflow.AddLogAsync(app.Id, "Committee Meeting Outcome Recorded", prev, app.Status.ToString(), request.Notes, request.UserId, "Officer", null, false, cancellationToken);
            }

            await _repo.SaveChangesAsync(cancellationToken);
            await _bpRepo.UnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
