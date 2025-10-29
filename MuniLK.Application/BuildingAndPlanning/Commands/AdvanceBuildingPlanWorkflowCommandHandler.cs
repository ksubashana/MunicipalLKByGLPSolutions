using MediatR;
using Microsoft.EntityFrameworkCore;
using MuniLK.Application.BuildingAndPlanning.Commands;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.Generic.Result;
using MuniLK.Domain.Constants.Flows;
using MuniLK.Domain.Entities;

namespace MuniLK.Application.BuildingAndPlanning.Commands
{
    /// <summary>
    /// Handler for advancing building plan applications through workflow stages
    /// </summary>
    public class AdvanceBuildingPlanWorkflowCommandHandler : IRequestHandler<AdvanceBuildingPlanWorkflowCommand, Result<BuildingPlanWorkflowResponse>>
    {
        private readonly IBuildingPlanRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public AdvanceBuildingPlanWorkflowCommandHandler(
            IBuildingPlanRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<Result<BuildingPlanWorkflowResponse>> Handle(AdvanceBuildingPlanWorkflowCommand request, CancellationToken cancellationToken)
        {
            // Get the application with workflow logs for validation
            var application = await _repository.GetByIdWithWorkflowLogsAsync(request.ApplicationId, cancellationToken);
            if (application == null)
            {
                return Result<BuildingPlanWorkflowResponse>.Failure("Building plan application not found");
            }

            var previousStatus = application.Status;
            var actionTaken = GetActionDescription(previousStatus, request.Decision);

            // Validate the workflow transition
            var validationResult = ValidateWorkflowTransition(application.Status, request.Decision);
            if (!validationResult.Succeeded)
            {
                return Result<BuildingPlanWorkflowResponse>.Failure(validationResult.Error);
            }

            // Apply the decision and update the application
            var newStatus = DetermineNewStatus(application.Status, request.Decision);
            application.Status = newStatus;

            // Update workflow-specific fields based on current stage
            UpdateWorkflowFields(application, previousStatus, request.Comments);

            // Create workflow log entry
            var workflowLog = new WorkflowLog
            {
                Id = Guid.NewGuid(),
                TenantId = application.TenantId ?? Guid.Empty,
                ApplicationId = application.Id,
                ActionTaken = actionTaken,
                PreviousStatus = previousStatus.ToString(),
                NewStatus = newStatus.ToString(),
                Remarks = request.Comments,
                PerformedByUserId = request.PerformedByUserId,
                PerformedByRole = request.PerformedByRole,
                AssignedToUserId = request.AssignedToUserId?.ToString(),
                IsSystemGenerated = false,
                PerformedAt = DateTime.UtcNow
            };

            application.WorkflowLogs.Add(workflowLog);

            // Save changes
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            var response = new BuildingPlanWorkflowResponse
            {
                ApplicationId = application.Id,
                PreviousStatus = previousStatus,
                NewStatus = newStatus,
                ActionTaken = actionTaken,
                ProcessedAt = workflowLog.PerformedAt,
                Comments = request.Comments
            };

            return Result<BuildingPlanWorkflowResponse>.Success(response);
        }

        /// <summary>
        /// Validates if the workflow transition is allowed from current status with given decision
        /// </summary>
        private static Result ValidateWorkflowTransition(BuildingAndPlanSteps currentStatus, ReviewDecision decision)
        {
            // Allow rejection from any stage except final stages
            if (decision == ReviewDecision.Rejected && 
                currentStatus != BuildingAndPlanSteps.Finalized && 
                currentStatus != BuildingAndPlanSteps.Rejected)
            {
                return Result.Success();
            }

            // Allow clarification requests from review stages
            if (decision == ReviewDecision.ClarificationRequired &&
                (
                 currentStatus == BuildingAndPlanSteps.PlanningCommitteeReview ||
                 currentStatus == BuildingAndPlanSteps.CommissionerApproval))
            {
                return Result.Success();
            }

            // Validate sequential approval workflow
            return currentStatus switch
            {
                BuildingAndPlanSteps.Submission when decision == ReviewDecision.Approved => Result.Success(),
                BuildingAndPlanSteps.ToReview when decision == ReviewDecision.Approved => Result.Success(),
                BuildingAndPlanSteps.PlanningCommitteeReview when decision == ReviewDecision.Approved => Result.Success(),
                BuildingAndPlanSteps.CommissionerApproval when decision == ReviewDecision.Approved => Result.Success(),
                BuildingAndPlanSteps.Finalized => Result.Failure("Application is already finalized"),
                BuildingAndPlanSteps.Rejected => Result.Failure("Application is already rejected"),
                _ => Result.Failure($"Invalid transition from {currentStatus} with decision {decision}")
            };
        }

        /// <summary>
        /// Determines the new status based on current status and decision
        /// </summary>
        private static BuildingAndPlanSteps DetermineNewStatus(BuildingAndPlanSteps currentStatus, ReviewDecision decision)
        {
            if (decision == ReviewDecision.Rejected)
                return BuildingAndPlanSteps.Rejected;

            if (decision == ReviewDecision.ClarificationRequired)
                return currentStatus; // Stay in the same stage for clarification

            // For approved decisions, advance to next stage
            return currentStatus switch
            {
                BuildingAndPlanSteps.Submission => BuildingAndPlanSteps.ToReview,
                BuildingAndPlanSteps.ToReview => BuildingAndPlanSteps.PlanningCommitteeReview,
                BuildingAndPlanSteps.PlanningCommitteeReview => BuildingAndPlanSteps.CommissionerApproval,
                BuildingAndPlanSteps.CommissionerApproval => BuildingAndPlanSteps.Finalized,
                _ => currentStatus
            };
        }

        /// <summary>
        /// Updates workflow-specific fields based on the stage being processed
        /// </summary>
        private static void UpdateWorkflowFields(BuildingPlanApplication application, BuildingAndPlanSteps stage, string? comments)
        {
            switch (stage)
            {
                case BuildingAndPlanSteps.PlanningCommitteeReview:
                    application.PlanningReport = comments;
                    break;
                case BuildingAndPlanSteps.CommissionerApproval:
                    application.CommissionerDecision = comments;
                    application.ApprovedOn = DateTime.UtcNow;
                    break;
            }
        }

        /// <summary>
        /// Gets a human-readable description of the action taken
        /// </summary>
        private static string GetActionDescription(BuildingAndPlanSteps currentStatus, ReviewDecision decision)
        {
            var stageDescription = currentStatus switch
            {
                BuildingAndPlanSteps.Submission => "Initial Review",
                BuildingAndPlanSteps.ToReview => "Administrative Review", 
                BuildingAndPlanSteps.PlanningCommitteeReview => "Planning Committee Review",
                BuildingAndPlanSteps.CommissionerApproval => "Commissioner Decision",
                _ => currentStatus.ToString()
            };

            var decisionDescription = decision switch
            {
                ReviewDecision.Approved => "Approved",
                ReviewDecision.Rejected => "Rejected",
                ReviewDecision.ClarificationRequired => "Clarification Requested",
                _ => decision.ToString()
            };

            return $"{stageDescription} - {decisionDescription}";
        }
    }
}