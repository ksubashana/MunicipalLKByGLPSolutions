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

            // Map minimal fields needed by the page
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
                Assignment = app.Assignment,
                SiteInspectionId = app.SiteInspectionId,
                SiteInspection = app.SiteInspection,
                PlanningCommitteeReviewId = app.PlanningCommitteeReviewId,
                PlanningCommitteeReview = app.PlanningCommitteeReview,
                Documents = new(),   // not needed for summary
                Workflow = new()     // not needed for summary
                // Status can be populated later if you have a Lookup projection
            };
        }
    }
}