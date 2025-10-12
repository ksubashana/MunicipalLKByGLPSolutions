using MediatR;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.Generic.Result;
using MuniLK.Domain.Constants.Flows;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.Commands
{
    /// <summary>
    /// Handler for orchestrating workflow operations on building plan applications
    /// Links child entities (Assignment, SiteInspection, PlanningCommitteeReview) to parent application
    /// and manages workflow status transitions
    /// </summary>
    public class WorkflowOrchestrationCommandHandler :
        IRequestHandler<AssignInspectionWorkflowCommand, Result>,
        IRequestHandler<CreateSiteInspectionWorkflowCommand, Result>,
        IRequestHandler<CreatePlanningCommitteeReviewWorkflowCommand, Result>
    {
        private readonly IBuildingPlanRepository _repository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWorkflowService _workflowService;

        public WorkflowOrchestrationCommandHandler(
            IBuildingPlanRepository repository,
            ICurrentUserService currentUserService,
            IWorkflowService workflowService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
            _workflowService = workflowService;
        }

        /// <summary>
        /// Handles assigning an inspection to a building plan application
        /// </summary>
        public async Task<Result> Handle(AssignInspectionWorkflowCommand request, CancellationToken cancellationToken)
        {
            // Get the building plan application
            var application = await _repository.GetByIdAsync(request.BuildingPlanApplicationId, cancellationToken);
            if (application == null)
                return Result.Failure("Building plan application not found");

            // Store previous status
            var previousStatus = application.Status;

            // Update the assignment foreign key
            application.AssignmentId = request.AssignmentId;

            // Update workflow status
            var newStatus = BuildingAndPlanSteps.AssignInspector;
            application.Status = newStatus;

            // Add workflow log
            var roles = _currentUserService.GetRoles();
            var rolesString = roles != null ? string.Join(",", roles) : null;

            await _workflowService.AddLogAsync(
                applicationId: application.Id,
                actionTaken: "Inspector Assigned",
                previousStatus: previousStatus.ToString(),
                newStatus: newStatus.ToString(),
                remarks: request.Remarks,
                performedByUserId: _currentUserService.UserId ?? "System",
                performedByRole: rolesString,
                assignedToUserId: request.AssignedUserId,
                isSystemGenerated: false,
                ct: cancellationToken);

            // Save changes
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        /// <summary>
        /// Handles creating a site inspection for a building plan application
        /// </summary>
        public async Task<Result> Handle(CreateSiteInspectionWorkflowCommand request, CancellationToken cancellationToken)
        {
            // Get the building plan application
            var application = await _repository.GetByIdAsync(request.BuildingPlanApplicationId, cancellationToken);
            if (application == null)
                return Result.Failure("Building plan application not found");

            // Store previous status
            var previousStatus = application.Status;

            // Update the site inspection foreign key
            application.SiteInspectionId = request.SiteInspectionId;

            // Update workflow status - move to next appropriate step
            var newStatus = BuildingAndPlanSteps.ToReview;
            application.Status = newStatus;

            // Add workflow log
            var roles = _currentUserService.GetRoles();
            var rolesString = roles != null ? string.Join(",", roles) : null;

            await _workflowService.AddLogAsync(
                applicationId: application.Id,
                actionTaken: "Site Inspection Completed",
                previousStatus: previousStatus.ToString(),
                newStatus: newStatus.ToString(),
                remarks: request.Remarks,
                performedByUserId: _currentUserService.UserId ?? "System",
                performedByRole: rolesString,
                assignedToUserId: request.AssignedUserId,
                isSystemGenerated: false,
                ct: cancellationToken);

            // Save changes
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        /// <summary>
        /// Handles creating a planning committee review for a building plan application
        /// </summary>
        public async Task<Result> Handle(CreatePlanningCommitteeReviewWorkflowCommand request, CancellationToken cancellationToken)
        {
            // Get the building plan application
            var application = await _repository.GetByIdAsync(request.BuildingPlanApplicationId, cancellationToken);
            if (application == null)
                return Result.Failure("Building plan application not found");

            // Store previous status
            var previousStatus = application.Status;

            // Update the planning committee review foreign key
            application.PlanningCommitteeReviewId = request.PlanningCommitteeReviewId;

            // Update workflow status - move to committee review
            var newStatus = BuildingAndPlanSteps.PlanningCommitteeReview;
            application.Status = newStatus;

            // Add workflow log
            var roles = _currentUserService.GetRoles();
            var rolesString = roles != null ? string.Join(",", roles) : null;

            await _workflowService.AddLogAsync(
                applicationId: application.Id,
                actionTaken: "Planning Committee Review Scheduled",
                previousStatus: previousStatus.ToString(),
                newStatus: newStatus.ToString(),
                remarks: request.Remarks,
                performedByUserId: _currentUserService.UserId ?? "System",
                performedByRole: rolesString,
                assignedToUserId: request.AssignedUserId,
                isSystemGenerated: false,
                ct: cancellationToken);

            // Save changes
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
