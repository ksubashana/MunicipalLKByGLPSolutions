using MuniLK.Domain.Constants.Flows;
using MuniLK.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MuniLK.Domain.Entities
{
    /// <summary>
    /// Planning Committee Review stores only review outcome & context linked to a scheduled meeting.
    /// Meeting metadata (date, chairperson, members, venue, etc.) lives in PlanningCommitteeMeeting & related member entities.
    /// </summary>
    public class PlanningCommitteeReview : IHasTenant
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }

        /// <summary>Associated Building Plan Application</summary>
        [Required]
        public Guid ApplicationId { get; set; }

        /// <summary>Linked committee meeting containing session details</summary>
        [Required]
        public Guid PlanningCommitteeMeetingId { get; set; }

        // Review Inputs (optional contextual data captured at review time)
        /// <summary>JSON list of inspection report identifiers reviewed</summary>
        public string? InspectionReportsReviewed { get; set; }
        /// <summary>JSON list of document type codes / IDs reviewed</summary>
        public string? DocumentsReviewed { get; set; }
        /// <summary>JSON list of external agencies consulted</summary>
        public string? ExternalAgenciesConsulted { get; set; }
        /// <summary>Was applicant or representative present</summary>
        public bool ApplicantRepresented { get; set; }
        /// <summary>Free-text summary of committee discussions</summary>
        public string? CommitteeDiscussionsSummary { get; set; }

        // Decision & Recommendations
        [Required]
        public CommitteeDecision CommitteeDecision { get; set; }
        /// <summary>Conditions imposed when approving with conditions</summary>
        public string? ConditionsImposed { get; set; }
        /// <summary>Reason supplied for rejection or deferral</summary>
        public string? ReasonForRejectionOrDeferral { get; set; }
        /// <summary>Path / blob reference for final recommendation document</summary>
        public string? FinalRecommendationDocumentPath { get; set; }

        // Audit & Sign-Off
        [Required]
        public string RecordedByOfficer { get; set; } = string.Empty;
        public DateTime ApprovalTimestamp { get; set; } = DateTime.UtcNow;
        /// <summary>JSON list of digital signatures</summary>
        public string? DigitalSignatures { get; set; }

        // Standard audit fields
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        // Navigation
        public BuildingPlanApplication? Application { get; set; }
        public PlanningCommitteeMeeting? PlanningCommitteeMeeting { get; set; }
    }
}