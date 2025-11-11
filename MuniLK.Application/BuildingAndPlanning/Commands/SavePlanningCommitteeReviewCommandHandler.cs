using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.Generic.Result;
using MuniLK.Domain.Entities;
using System.Text.Json;

namespace MuniLK.Application.BuildingAndPlanning.Commands
{
    /// <summary>
    /// Handler for saving/updating planning committee reviews (simplified review-only model)
    /// </summary>
    public class SavePlanningCommitteeReviewCommandHandler : IRequestHandler<SavePlanningCommitteeReviewCommand, Result<PlanningCommitteeReviewResponse>>
    {
        private readonly IPlanningCommitteeReviewRepository _repository;
        private readonly IBuildingPlanRepository _buildingPlanRepository;
        private readonly ICurrentTenantService _tenantService;
        private readonly IWorkflowService _workflowService;

        public SavePlanningCommitteeReviewCommandHandler(
            IPlanningCommitteeReviewRepository repository,
            IBuildingPlanRepository buildingPlanRepository,
            ICurrentTenantService tenantService,
            IWorkflowService workflowService)
        {
            _repository = repository;
            _buildingPlanRepository = buildingPlanRepository;
            _tenantService = tenantService;
            _workflowService = workflowService;
        }

        public async Task<Result<PlanningCommitteeReviewResponse>> Handle(
            SavePlanningCommitteeReviewCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var tenantId = _tenantService.GetTenantId()
                    ?? throw new InvalidOperationException("Tenant not found.");

                // Validate application exists
                var application = await _buildingPlanRepository.GetByIdAsync(request.Request.ApplicationId, cancellationToken);
                if (application == null)
                {
                    return Result<PlanningCommitteeReviewResponse>.Failure("Building plan application not found");
                }

                // Load existing review if any (one per application)
                var existingReview = await _repository.GetByApplicationIdAsync(request.Request.ApplicationId, cancellationToken);

                PlanningCommitteeReview review;
                var isNew = existingReview == null;
                if (isNew)
                {
                    review = new PlanningCommitteeReview
                    {
                        Id = Guid.NewGuid(),
                        TenantId = tenantId,
                        ApplicationId = request.Request.ApplicationId,
                        PlanningCommitteeMeetingId = request.Request.PlanningCommitteeMeetingId,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = request.UserId
                    };
                }
                else
                {
                    review = existingReview;
                    review.PlanningCommitteeMeetingId = request.Request.PlanningCommitteeMeetingId; // allow re-link if needed
                    review.ModifiedDate = DateTime.UtcNow;
                    review.ModifiedBy = request.UserId;
                }

                MapRequestToEntity(request.Request, review);

                if (isNew)
                    await _repository.AddAsync(review, cancellationToken);
                else
                    await _repository.UpdateAsync(review, cancellationToken);

                // Workflow log (status transition external; here just record action)
                await _workflowService.AddLogAsync(
                    applicationId: request.Request.ApplicationId,
                    actionTaken: GetActionDescription(request.Request.CommitteeDecision),
                    previousStatus: application.Status.ToString(),
                    newStatus: application.Status.ToString(), // unchanged here; orchestration command handles status
                    remarks: BuildRemarks(request.Request),
                    performedByUserId: request.UserId,
                    performedByRole: "CommitteeMember",
                    assignedToUserId: null,
                    isSystemGenerated: false,
                    ct: cancellationToken);

                await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

                var response = MapEntityToResponse(review);
                return Result<PlanningCommitteeReviewResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<PlanningCommitteeReviewResponse>.Failure($"Error saving planning committee review: {ex.Message}");
            }
        }

        private static string BuildRemarks(PlanningCommitteeReviewRequest req)
        {
            var summary = string.IsNullOrWhiteSpace(req.CommitteeDiscussionsSummary)
                ? string.Empty
                : $" Summary: {req.CommitteeDiscussionsSummary[..Math.Min(200, req.CommitteeDiscussionsSummary.Length)]}";
            return $"Decision: {req.CommitteeDecision}.{summary}";
        }

        /// <summary>Map simplified request DTO to entity.</summary>
        private static void MapRequestToEntity(PlanningCommitteeReviewRequest request, PlanningCommitteeReview entity)
        {
            entity.InspectionReportsReviewed = request.InspectionReportsReviewed.Any()
                ? JsonSerializer.Serialize(request.InspectionReportsReviewed) : null;
            entity.DocumentsReviewed = request.DocumentsReviewed.Any()
                ? JsonSerializer.Serialize(request.DocumentsReviewed) : null;
            entity.ApplicantRepresented = request.ApplicantRepresented;
            entity.ExternalAgenciesConsulted = request.ExternalAgenciesConsulted.Any()
                ? JsonSerializer.Serialize(request.ExternalAgenciesConsulted) : null;
            entity.CommitteeDiscussionsSummary = request.CommitteeDiscussionsSummary;
            entity.CommitteeDecision = request.CommitteeDecision;
            entity.ConditionsImposed = request.ConditionsImposed;
            entity.ReasonForRejectionOrDeferral = request.ReasonForRejectionOrDeferral;
            entity.RecordedByOfficer = request.RecordedByOfficer;
            entity.ApprovalTimestamp = DateTime.UtcNow;
            entity.DigitalSignatures = request.DigitalSignatures.Any()
                ? JsonSerializer.Serialize(request.DigitalSignatures) : null;
        }

        /// <summary>Map entity to response DTO.</summary>
        private static PlanningCommitteeReviewResponse MapEntityToResponse(PlanningCommitteeReview entity)
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

        private static string GetActionDescription(Domain.Constants.Flows.CommitteeDecision decision) => decision switch
        {
            Domain.Constants.Flows.CommitteeDecision.Approve => "Committee Approved",
            Domain.Constants.Flows.CommitteeDecision.ApproveWithConditions => "Committee Approved with Conditions",
            Domain.Constants.Flows.CommitteeDecision.Reject => "Committee Rejected",
            Domain.Constants.Flows.CommitteeDecision.DeferForClarifications => "Committee Deferred for Clarifications",
            _ => "Committee Review Updated"
        };
    }
}