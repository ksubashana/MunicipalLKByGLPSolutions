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

            string? nextStage = canProceedToCommittee ? BuildingAndPlanSteps.PlanningCommitteeReview.ToString() : null;

            return new BuildingPlanWorkflowSnapshot
            {
                ApplicationId = app.Id,
                ApplicationNumber = app.ApplicationNumber,
                CurrentStatus = app.Status,
                SiteInspectionStatus = inspectionStatus,
                HasAssignment = hasAssignment,
                AssignmentDate = assignmentDate,
                IsAssignmentExpired = assignmentExpired,
                IsInspectionCompleted = isInspectionCompleted,
                CanProceedToCommittee = canProceedToCommittee,
                SubmittedOn = app.SubmittedOn,
                NextStage = nextStage
            };
        }
    }
}
