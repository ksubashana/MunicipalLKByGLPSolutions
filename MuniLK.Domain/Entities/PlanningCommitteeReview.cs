using MuniLK.Domain.Constants.Flows;
using MuniLK.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MuniLK.Domain.Entities
{
    /// <summary>
    /// Entity for Planning Committee Review in building plan approval workflow
    /// </summary>
    public class PlanningCommitteeReview : IHasTenant
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }

        // 1. Committee Session Metadata
        [Required]
        public DateTime MeetingDate { get; set; }

        [Required]
        public CommitteeType CommitteeType { get; set; }

        [Required]
        public string MeetingReferenceNo { get; set; } = default!;

        [Required]
        public string ChairpersonName { get; set; } = default!;

        /// <summary>
        /// JSON serialized list of committee members with their designations
        /// </summary>
        [Required]
        public string MembersPresent { get; set; } = default!;

        // 2. Review Inputs
        /// <summary>
        /// JSON serialized list of inspection report IDs reviewed
        /// </summary>
        public string? InspectionReportsReviewed { get; set; }

        /// <summary>
        /// JSON serialized list of document types reviewed
        /// </summary>
        public string? DocumentsReviewed { get; set; }

        public bool ApplicantRepresented { get; set; }

        /// <summary>
        /// JSON serialized list of external agencies consulted
        /// </summary>
        public string? ExternalAgenciesConsulted { get; set; }

        public string? CommitteeDiscussionsSummary { get; set; }

        // 3. Decision & Recommendations
        [Required]
        public CommitteeDecision CommitteeDecision { get; set; }

        public string? ConditionsImposed { get; set; }

        public string? ReasonForRejectionOrDeferral { get; set; }

        /// <summary>
        /// Path to the uploaded final recommendation document
        /// </summary>
        public string? FinalRecommendationDocumentPath { get; set; }

        // 4. Audit & Sign-Off
        [Required]
        public string RecordedByOfficer { get; set; } = default!;

        public DateTime ApprovalTimestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// JSON serialized list of digital signatures (if available)
        /// </summary>
        public string? DigitalSignatures { get; set; }

        // Standard audit fields
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        // Navigation Properties
        public BuildingPlanApplication? Application { get; set; }
    }
}