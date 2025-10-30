using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Domain.Constants.Flows;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.Queries
{
    public class GetBuildingPlanWorkflowSnapshotQueryHandler : IRequestHandler<GetBuildingPlanWorkflowSnapshotQuery, BuildingPlanWorkflowSnapshot?>
    {
        private readonly IBuildingPlanRepository _repository;
        public GetBuildingPlanWorkflowSnapshotQueryHandler(IBuildingPlanRepository repository) => _repository = repository;

        public async Task<BuildingPlanWorkflowSnapshot?> Handle(GetBuildingPlanWorkflowSnapshotQuery request, CancellationToken cancellationToken)
        {
            var app = await _repository.GetByIdWithChildrenAsync(request.ApplicationId, cancellationToken);
            if (app == null) return null;

            InspectionStatus? inspectionStatus = app.SiteInspection?.Status;
            if (app.SiteInspection?.FinalRecommendation is FinalRecommendation.ApproveAsSubmitted or FinalRecommendation.ApproveWithModifications)
            {
                inspectionStatus = InspectionStatus.Approve; // normalize
            }
            else if (app.SiteInspection?.FinalRecommendation == FinalRecommendation.ReInspectionRequired)
            {
                inspectionStatus = InspectionStatus.ReInspectionRequired;
            }
            else if (app.SiteInspection?.FinalRecommendation == FinalRecommendation.Reject)
            {
                inspectionStatus = InspectionStatus.Reject;
            }

            bool isInspectionCompleted = inspectionStatus is InspectionStatus.Approve or InspectionStatus.Reject;
            bool canProceedToCommittee = inspectionStatus == InspectionStatus.Approve;
            var assignmentDate = app.Assignment?.AssignmentDate;
            bool hasAssignment = app.AssignmentId.HasValue;
            bool assignmentExpired = assignmentDate.HasValue && assignmentDate.Value.Date < DateTime.Today && !isInspectionCompleted;

            // Committee scheduling context
            bool hasCommitteeSchedule = app.PlanningCommitteeReviewId.HasValue;
            DateTime? committeeMeetingDate = app.PlanningCommitteeReview?.MeetingDate;
            CommitteeDecision? committeeDecision = hasCommitteeSchedule ? app.PlanningCommitteeReview?.CommitteeDecision : null;

            string? nextStage = null;
            // Derive next stage considering new AssignToCommittee step
            if (app.Status == BuildingAndPlanSteps.ToReview && canProceedToCommittee && !hasCommitteeSchedule)
            {
                nextStage = BuildingAndPlanSteps.AssignToCommittee.ToString();
            }
            else if (app.Status == BuildingAndPlanSteps.AssignToCommittee && hasCommitteeSchedule && (committeeDecision == Domain.Constants.Flows.CommitteeDecision.Pending))
            {
                nextStage = BuildingAndPlanSteps.PlanningCommitteeReview.ToString();
            }
            else if (app.Status == BuildingAndPlanSteps.PlanningCommitteeReview && committeeDecision is Domain.Constants.Flows.CommitteeDecision.Approve or Domain.Constants.Flows.CommitteeDecision.ApproveWithConditions)
            {
                nextStage = BuildingAndPlanSteps.CommissionerApproval.ToString();
            }
            else if (app.Status == BuildingAndPlanSteps.PlanningCommitteeReview && committeeDecision is Domain.Constants.Flows.CommitteeDecision.Reject)
            {
                nextStage = BuildingAndPlanSteps.Rejected.ToString();
            }
            else if (app.Status == BuildingAndPlanSteps.PlanningCommitteeReview && committeeDecision is Domain.Constants.Flows.CommitteeDecision.DeferForClarifications)
            {
                nextStage = BuildingAndPlanSteps.PlanningCommitteeReview.ToString(); // stays in review until clarified
            }
            else if (canProceedToCommittee && app.Status == BuildingAndPlanSteps.ToReview)
            {
                nextStage = BuildingAndPlanSteps.AssignToCommittee.ToString();
            }

            return new BuildingPlanWorkflowSnapshot
            {
                ApplicationId = app.Id,
                ApplicationNumber = app.ApplicationNumber ?? string.Empty,
                CurrentStatus = app.Status,
                SiteInspectionStatus = inspectionStatus,
                HasAssignment = hasAssignment,
                AssignmentDate = assignmentDate,
                IsAssignmentExpired = assignmentExpired,
                IsInspectionCompleted = isInspectionCompleted,
                CanProceedToCommittee = canProceedToCommittee,
                HasCommitteeSchedule = hasCommitteeSchedule,
                CommitteeMeetingDate = committeeMeetingDate,
                CommitteeDecision = committeeDecision,
                SubmittedOn = app.SubmittedOn,
                NextStage = nextStage
            };
        }
    }
}
