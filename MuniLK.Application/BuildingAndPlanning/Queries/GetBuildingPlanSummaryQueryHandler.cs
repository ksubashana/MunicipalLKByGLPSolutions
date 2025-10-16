using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.Queries
{
    public class GetBuildingPlanSummaryQueryHandler : IRequestHandler<GetBuildingPlanSummaryQuery, BuildingPlanResponse?>
    {
        private readonly IBuildingPlanRepository _repo;

        public GetBuildingPlanSummaryQueryHandler(IBuildingPlanRepository repo)
        {
            _repo = repo;
        }

        public async Task<BuildingPlanResponse?> Handle(GetBuildingPlanSummaryQuery request, CancellationToken ct)
        {
            var app = await _repo.GetByIdWithChildrenAsync(request.Id, ct);
            if (app is null) return null;

            // Map minimal fields needed by the page; project navigation entities to lightweight DTOs
            return new BuildingPlanResponse
            {
                Id = app.Id,
                ApplicationNumber = app.ApplicationNumber ?? string.Empty,
                ApplicantContactId = app.ApplicantContactId,
                PropertyId = app.PropertyId,
                BuildingPurpose = app.BuildingPurpose,
                NoOfFloors = app.NoOfFloors,
                SubmittedOn = app.SubmittedOn,
                AssignmentId = app.AssignmentId,
                Assignment = app.Assignment is null ? null : new MuniLK.Application.Assignments.DTOs.AssignmentResponse
                {
                    Id = app.Assignment.Id,
                    AssignedToUserId = app.Assignment.AssignedTo,
                    AssignedToName = string.Empty, // populate via join/user service if needed
                    AssignedByUserId = app.Assignment.AssignedBy,
                    AssignedByName = string.Empty,
                    ModuleId = app.Assignment.ModuleId,
                    ModuleName = string.Empty,
                    EntityId = app.Assignment.EntityId,
                    EntityType = app.Assignment.EntityType,
                    EntityReference = app.ApplicationNumber ?? string.Empty,
                    AssignmentDate = app.Assignment.AssignmentDate,
                    DueDate = app.Assignment.DueDate,
                    TaskType = app.Assignment.TaskType,
                    Notes = app.Assignment.Notes ?? string.Empty,
                    IsCompleted = app.Assignment.IsCompleted,
                    CompletedAt = app.Assignment.CompletedAt,
                    Outcome = app.Assignment.Outcome,
                    OutcomeRemarks = app.Assignment.OutcomeRemarks,
                    FeatureId = app.Assignment.FeatureId
                },
                SiteInspectionId = app.SiteInspectionId,
                SiteInspection = app.SiteInspection is null ? null : new SiteInspectionResponse
                {
                    Id = app.SiteInspection.Id,
                    ApplicationId = app.SiteInspection.ApplicationId,
                    InspectionId = app.SiteInspection.InspectionId,
                    Status = app.SiteInspection.Status,
                    Remarks = app.SiteInspection.Remarks,
                    InspectionDate = app.SiteInspection.InspectionDate,
                    OfficersPresent = app.SiteInspection.OfficersPresent,
                    GpsCoordinates = app.SiteInspection.GpsCoordinates,
                    // PhotoUrls, SiteConditions, ComplianceChecks left empty in summary context
                    RequiredModifications = app.SiteInspection.RequiredModifications,
                    ClearanceOptionItemIds = null, // parse JSON only in detailed query
                    FinalRecommendation = app.SiteInspection.FinalRecommendation,
                    CreatedDate = app.SiteInspection.CreatedDate,
                    CreatedBy = app.SiteInspection.CreatedBy,
                    ModifiedDate = app.SiteInspection.ModifiedDate,
                    ModifiedBy = app.SiteInspection.ModifiedBy
                },
                PlanningCommitteeReviewId = app.PlanningCommitteeReviewId,
                PlanningCommitteeReview = app.PlanningCommitteeReview is null ? null : new PlanningCommitteeReviewResponse
                {
                    Id = app.PlanningCommitteeReview.Id,
                    ApplicationId = app.PlanningCommitteeReview.ApplicationId,
                    MeetingDate = app.PlanningCommitteeReview.MeetingDate,
                    CommitteeType = app.PlanningCommitteeReview.CommitteeType,
                    MeetingReferenceNo = app.PlanningCommitteeReview.MeetingReferenceNo,
                    ChairpersonName = app.PlanningCommitteeReview.ChairpersonName,
                    // MembersPresent, DocumentsReviewed etc. require JSON deserialization -> omit in summary
                    CommitteeDecision = app.PlanningCommitteeReview.CommitteeDecision,
                    // Other optional textual fields omitted for brevity in summary
                    RecordedByOfficer = app.PlanningCommitteeReview.RecordedByOfficer,
                    ApprovalTimestamp = app.PlanningCommitteeReview.ApprovalTimestamp,
                    CreatedDate = app.PlanningCommitteeReview.CreatedDate,
                    CreatedBy = app.PlanningCommitteeReview.CreatedBy,
                    ModifiedDate = app.PlanningCommitteeReview.ModifiedDate,
                    ModifiedBy = app.PlanningCommitteeReview.ModifiedBy
                },
                Documents = new(),   // not needed for summary
                Workflow = new()     // not needed for summary
                // Status can be populated later if you have a Lookup projection
            };
        }
    }
}