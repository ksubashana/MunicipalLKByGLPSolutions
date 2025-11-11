using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.Generic.Result;
using System.Text.Json;

namespace MuniLK.Application.BuildingAndPlanning.Queries
{
    /// <summary>
    /// Handler for getting planning committee review (simplified review-only model)
    /// </summary>
    public class GetPlanningCommitteeReviewQueryHandler : IRequestHandler<GetPlanningCommitteeReviewQuery, Result<PlanningCommitteeReviewResponse?>>
    {
        private readonly IPlanningCommitteeReviewRepository _repository;

        public GetPlanningCommitteeReviewQueryHandler(IPlanningCommitteeReviewRepository repository) => _repository = repository;

        public async Task<Result<PlanningCommitteeReviewResponse?>> Handle(GetPlanningCommitteeReviewQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var review = await _repository.GetByApplicationIdAsync(request.ApplicationId, cancellationToken);
                if (review == null) return Result<PlanningCommitteeReviewResponse?>.Success(null);
                var response = MapEntityToResponse(review);
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