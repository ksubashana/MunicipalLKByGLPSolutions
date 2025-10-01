using MuniLK.Domain.Constants.Flows;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MuniLK.Application.BuildingAndPlanning.DTOs
{
    /// <summary>
    /// DTO for Planning Committee Review Request
    /// </summary>
    public class PlanningCommitteeReviewRequest
    {
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

        [Required]
        public List<CommitteeMember> MembersPresent { get; set; } = new();

        // 2. Review Inputs
        public List<string> InspectionReportsReviewed { get; set; } = new();
        public List<string> DocumentsReviewed { get; set; } = new();
        
        public bool ApplicantRepresented { get; set; }
        
        public List<string> ExternalAgenciesConsulted { get; set; } = new();
        
        public string? CommitteeDiscussionsSummary { get; set; }

        // 3. Decision & Recommendations
        [Required]
        public CommitteeDecision CommitteeDecision { get; set; }

        public string? ConditionsImposed { get; set; }
        
        public string? ReasonForRejectionOrDeferral { get; set; }
        
        //public IFormFile? FinalRecommendationDocument { get; set; }

        // 4. Audit & Sign-Off
        [Required]
        public string RecordedByOfficer { get; set; } = default!;
        
        public List<string> DigitalSignatures { get; set; } = new();
    }

    /// <summary>
    /// DTO for Planning Committee Review Response
    /// </summary>
    public class PlanningCommitteeReviewResponse
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }

        // Committee Session Metadata
        public DateTime MeetingDate { get; set; }
        public CommitteeType CommitteeType { get; set; }
        public string MeetingReferenceNo { get; set; } = default!;
        public string ChairpersonName { get; set; } = default!;
        public List<CommitteeMember> MembersPresent { get; set; } = new();

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
        public string RecordedByOfficer { get; set; } = default!;
        public DateTime ApprovalTimestamp { get; set; }
        public List<string> DigitalSignatures { get; set; } = new();

        // Audit fields
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }

    /// <summary>
    /// DTO for Committee Member information
    /// </summary>
    public class CommitteeMember
    {
        [Required]
        public string Name { get; set; } = default!;
        
        [Required]
        public string Designation { get; set; } = default!;
    }
}