using MediatR;
using MuniLK.Application.Generic.Result;
using MuniLK.Application.PlanningCommitteeMeetings.DTOs;
using MuniLK.Application.PlanningCommitteeMeetings.Interfaces;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Domain.Constants.Flows;
using MuniLK.Domain.Entities;
using MuniLK.Applications.Interfaces;
using MuniLK.Application.Tenants; // for IContactRepository

namespace MuniLK.Application.PlanningCommitteeMeetings.Commands
{
    public record AssignApplicationToMeetingCommand(Guid MeetingId, Guid ApplicationId, string UserId) : IRequest<Result>;

    public class AssignApplicationToMeetingCommandHandler : IRequestHandler<AssignApplicationToMeetingCommand, Result>
    {
        private readonly IPlanningCommitteeMeetingRepository _repo;
        private readonly IBuildingPlanRepository _bpRepo;
        private readonly IWorkflowService _workflow;
        private readonly IEmailService _emailService;
        private readonly IContactRepository _contactRepository;
        private readonly ICurrentTenantService _tenantRepository;

        public AssignApplicationToMeetingCommandHandler(
            IPlanningCommitteeMeetingRepository repo,
            IBuildingPlanRepository bpRepo,
            IWorkflowService workflow,
            IEmailService emailService,
            IContactRepository contactRepository,
            ICurrentTenantService tenantRepository)
        {
            _repo = repo;
            _bpRepo = bpRepo;
            _workflow = workflow;
            _emailService = emailService;
            _contactRepository = contactRepository;
            _tenantRepository = tenantRepository;
        }

        public async Task<Result> Handle(AssignApplicationToMeetingCommand request, CancellationToken cancellationToken)
        {
            var meeting = await _repo.GetByIdAsync(request.MeetingId, cancellationToken);
            if (meeting == null || meeting.IsDeleted) return Result.Failure("Meeting not found");
            if (meeting.Status != PlanningCommitteeMeetingStatus.Scheduled) return Result.Failure("Cannot assign to a non-scheduled meeting");
            var tenantId = _tenantRepository.GetTenantId();

            // Add link if not exists
            if (!meeting.Applications.Any(a => a.BuildingPlanApplicationId == request.ApplicationId && !a.IsDeleted))
            {
                await _repo.AddApplicationAsync(new PlanningCommitteeMeetingApplication { Id = Guid.NewGuid(), TenantId= tenantId, PlanningCommitteeMeetingId = meeting.Id, BuildingPlanApplicationId = request.ApplicationId }, cancellationToken);
            }

            var app = await _bpRepo.GetForUpdateAsync(request.ApplicationId, cancellationToken);
            if (app == null) return Result.Failure("Application not found");
            var prev = app.Status.ToString();
            if (app.Status == BuildingAndPlanSteps.ToReview) app.Status = BuildingAndPlanSteps.AssignToCommittee;
            await _workflow.AddLogAsync(app.Id, "Application Assigned To Committee Meeting", prev, app.Status.ToString(), null, request.UserId, "Officer", null, false, cancellationToken);

            await _repo.SaveChangesAsync(cancellationToken);
            await _bpRepo.UnitOfWork.SaveChangesAsync(cancellationToken);

            // Send notification email to applicant (best-effort)
            try
            {
                var contact = await _contactRepository.GetByIdAsync(app.ApplicantContactId);
                if (contact != null && !string.IsNullOrWhiteSpace(contact.Email))
                {
                    await _emailService.SendCommitteeMeetingAssignmentEmailAsync(
                        contact.Email,
                        app.ApplicationNumber ?? app.Id.ToString(),
                        meeting.StartTime,
                        meeting.EndTime,
                        meeting.Venue,
                        "Chairperson" // Could be resolved with contact repo if chairperson is also a contact
                    );
                }
            }
            catch
            {
                // Swallow exceptions to avoid failing the main operation
            }

            return Result.Success();
        }
    }
}
