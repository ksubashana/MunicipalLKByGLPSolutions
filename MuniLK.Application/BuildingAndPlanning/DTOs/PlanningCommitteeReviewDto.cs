using MuniLK.Domain.Constants.Flows;
using System.ComponentModel.DataAnnotations;

namespace MuniLK.Application.BuildingAndPlanning.DTOs
{
    /// <summary>
    /// Request DTO for Planning Committee Review (review-only; meeting details stored in PlanningCommitteeMeeting).
    /// </summary>
    public class PlanningCommitteeReviewRequest
    {
        [Required]
        public Guid ApplicationId { get; set; }

        [Required]
        public Guid PlanningCommitteeMeetingId { get; set; }

        // Review Inputs
        public List<string> InspectionReportsReviewed { get; set; } = new();
        public List<string> DocumentsReviewed { get; set; } = new();
        public bool ApplicantRepresented { get; set; }
        public List<string> ExternalAgenciesConsulted { get; set; } = new();
        public string? CommitteeDiscussionsSummary { get; set; }

        // Decision & Recommendations
        [Required]
        public CommitteeDecision CommitteeDecision { get; set; }
        public string? ConditionsImposed { get; set; }
        public string? ReasonForRejectionOrDeferral { get; set; }

        // Audit & Sign-Off
        [Required]
        public string RecordedByOfficer { get; set; } = string.Empty;
        public List<string> DigitalSignatures { get; set; } = new();
    }

    /// <summary>
    /// Response DTO for Planning Committee Review.
    /// </summary>
    public class PlanningCommitteeReviewResponse
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid PlanningCommitteeMeetingId { get; set; }

        // Review Inputs
        public List<string> InspectionReportsReviewed { get; set; } = new();
        public List<string> DocumentsReviewed { get; set; } = new();
        public bool ApplicantRepresented { get; set; }
        public List<string> ExternalAgenciesConsulted { get; set; } = new();
        public string? CommitteeDiscussionsSummary { get; set; }

        // Decision & Recommendations
        public CommitteeDecision CommitteeDecision { get; set; }
        public string? ConditionsImposed { get; set; }
        public string? ReasonForRejectionOrDeferral { get; set; }
        public string? FinalRecommendationDocumentUrl { get; set; }

        // Audit & Sign-Off
        public string RecordedByOfficer { get; set; } = string.Empty;
        public DateTime ApprovalTimestamp { get; set; }
        public List<string> DigitalSignatures { get; set; } = new();

        // Audit fields
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        // Option Selection IDs
        public List<Guid> InspectionReportOptionItemIds { get; set; } = new();
        public List<Guid> DocumentsReviewedOptionItemIds { get; set; } = new();
        public List<Guid> ExternalAgencyOptionItemIds { get; set; } = new();
    }
}