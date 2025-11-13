using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.Generic.Result;
using System.Text.Json;
using MuniLK.Domain.Constants; // for LookupCategoryNames

namespace MuniLK.Application.BuildingAndPlanning.Queries
{
    /// <summary>
    /// Handler for getting planning committee review (simplified review-only model)
    /// </summary>
    public class GetPlanningCommitteeReviewQueryHandler : IRequestHandler<GetPlanningCommitteeReviewQuery, Result<PlanningCommitteeReviewResponse?>>
    {
        private readonly IPlanningCommitteeReviewRepository _repository;
        private readonly IEntityOptionSelectionRepository _optionSelections;
        private const string CommonEntityType = "PlanningCommittee"; // unified entity type discriminator

        public GetPlanningCommitteeReviewQueryHandler(IPlanningCommitteeReviewRepository repository, IEntityOptionSelectionRepository optionSelections)
        {
            _repository = repository;
            _optionSelections = optionSelections;
        }

        public async Task<Result<PlanningCommitteeReviewResponse?>> Handle(GetPlanningCommitteeReviewQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var review = await _repository.GetByApplicationIdAsync(request.ApplicationId, cancellationToken);
                if (review == null) return Result<PlanningCommitteeReviewResponse?>.Success(null);
                var response = MapEntityToResponse(review);

                // Load option selections scoped by ApplicationId + CommonEntityType + ModuleId (moduleId not stored on review; derive from application if needed externally)
                // We only have ApplicationId here; consumer must supply module id when persisting selections.
                // Attempt to load all selections for this application under common entity type across any module (simplest approach)
                var allSelections = await _optionSelections.GetSelectionsAsync(review.ApplicationId, CommonEntityType, review.ApplicationId, cancellationToken);
                if (allSelections.Any())
                {
                    response.InspectionReportOptionItemIds = allSelections
                        .Where(s => string.Equals(s.LookupCategoryName, LookupCategoryNames.InspectionReports.ToString(), StringComparison.OrdinalIgnoreCase))
                        .Select(s => s.LookupId).Distinct().ToList();
                    response.DocumentsReviewedOptionItemIds = allSelections
                        .Where(s => string.Equals(s.LookupCategoryName, LookupCategoryNames.PlanningReviewDocuments.ToString(), StringComparison.OrdinalIgnoreCase))
                        .Select(s => s.LookupId).Distinct().ToList();
                    response.ExternalAgencyOptionItemIds = allSelections
                        .Where(s => string.Equals(s.LookupCategoryName, LookupCategoryNames.ClearanceTypes.ToString(), StringComparison.OrdinalIgnoreCase))
                        .Select(s => s.LookupId).Distinct().ToList();
                }
                return Result<PlanningCommitteeReviewResponse?>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<PlanningCommitteeReviewResponse?>.Failure($"Error retrieving planning committee review: {ex.Message}");
            }
        }

        private static PlanningCommitteeReviewResponse MapEntityToResponse(Domain.Entities.PlanningCommitteeReview entity)
        {
            return new PlanningCommitteeReviewResponse
            {
                Id = entity.Id,
                ApplicationId = entity.ApplicationId,
                PlanningCommitteeMeetingId = entity.PlanningCommitteeMeetingId,
                InspectionReportsReviewed = string.IsNullOrEmpty(entity.InspectionReportsReviewed)
                    ? new() : JsonSerializer.Deserialize<List<string>>(entity.InspectionReportsReviewed) ?? new(),
                DocumentsReviewed = string.IsNullOrEmpty(entity.DocumentsReviewed)
                    ? new() : JsonSerializer.Deserialize<List<string>>(entity.DocumentsReviewed) ?? new(),
                ApplicantRepresented = entity.ApplicantRepresented,
                ExternalAgenciesConsulted = string.IsNullOrEmpty(entity.ExternalAgenciesConsulted)
                    ? new() : JsonSerializer.Deserialize<List<string>>(entity.ExternalAgenciesConsulted) ?? new(),
                CommitteeDiscussionsSummary = entity.CommitteeDiscussionsSummary,
                CommitteeDecision = entity.CommitteeDecision,
                ConditionsImposed = entity.ConditionsImposed,
                ReasonForRejectionOrDeferral = entity.ReasonForRejectionOrDeferral,
                FinalRecommendationDocumentUrl = entity.FinalRecommendationDocumentPath,
                RecordedByOfficer = entity.RecordedByOfficer,
                ApprovalTimestamp = entity.ApprovalTimestamp,
                DigitalSignatures = string.IsNullOrEmpty(entity.DigitalSignatures)
                    ? new() : JsonSerializer.Deserialize<List<string>>(entity.DigitalSignatures) ?? new(),
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedBy,
                ModifiedDate = entity.ModifiedDate,
                ModifiedBy = entity.ModifiedBy
            };
        }
    }
}