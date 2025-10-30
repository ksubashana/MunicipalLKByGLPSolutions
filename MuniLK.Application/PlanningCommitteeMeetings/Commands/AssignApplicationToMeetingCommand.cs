using MediatR;
using MuniLK.Application.Generic.Result;
using MuniLK.Application.PlanningCommitteeMeetings.DTOs;
using MuniLK.Application.PlanningCommitteeMeetings.Interfaces;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Domain.Constants.Flows;
using MuniLK.Domain.Entities;

namespace MuniLK.Application.PlanningCommitteeMeetings.Commands
{
    public record AssignApplicationToMeetingCommand(Guid MeetingId, Guid ApplicationId, string UserId) : IRequest<Result>;

    public class AssignApplicationToMeetingCommandHandler : IRequestHandler<AssignApplicationToMeetingCommand, Result>
    {
        private readonly IPlanningCommitteeMeetingRepository _repo;
        private readonly IBuildingPlanRepository _bpRepo;
        private readonly IWorkflowService _workflow;

        public AssignApplicationToMeetingCommandHandler(IPlanningCommitteeMeetingRepository repo, IBuildingPlanRepository bpRepo, IWorkflowService workflow)
        { _repo = repo; _bpRepo = bpRepo; _workflow = workflow; }

        public async Task<Result> Handle(AssignApplicationToMeetingCommand request, CancellationToken cancellationToken)
        {
            var meeting = await _repo.GetByIdAsync(request.MeetingId, cancellationToken);
            if (meeting == null || meeting.IsDeleted) return Result.Failure("Meeting not found");
            if (meeting.Status != PlanningCommitteeMeetingStatus.Scheduled) return Result.Failure("Cannot assign to a non-scheduled meeting");

            // Add link if not exists
            if (!meeting.Applications.Any(a => a.BuildingPlanApplicationId == request.ApplicationId && !a.IsDeleted))
            {
                await _repo.AddApplicationAsync(new PlanningCommitteeMeetingApplication { Id = Guid.NewGuid(), MeetingId = meeting.Id, BuildingPlanApplicationId = request.ApplicationId }, cancellationToken);
            }

            var app = await _bpRepo.GetForUpdateAsync(request.ApplicationId, cancellationToken);
            if (app == null) return Result.Failure("Application not found");
            var prev = app.Status.ToString();
            if (app.Status == BuildingAndPlanSteps.ToReview) app.Status = BuildingAndPlanSteps.AssignToCommittee;
            await _workflow.AddLogAsync(app.Id, "Application Assigned To Committee Meeting", prev, app.Status.ToString(), null, request.UserId, "Officer", null, false, cancellationToken);

            await _repo.SaveChangesAsync(cancellationToken);
            await _bpRepo.UnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
