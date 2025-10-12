using MuniLK.Application.Contact.DTOs;
using MuniLK.Application.Documents.DTOs;
using MuniLK.Application.PropertiesLK.DTOs;
using MuniLK.Domain.Constants.Flows;
using MuniLK.Domain.Entities;

namespace MuniLK.Application.BuildingAndPlanning.DTOs
{
    /// <summary>
    /// Enhanced DTO for Building Plan Application with workflow data
    /// </summary>
    public class BuildingPlanApplicationDto
    {
        public Guid Id { get; set; }
        public string ApplicationNumber { get; set; } = default!;
        public BuildingAndPlanSteps Status { get; set; }
        
        // Basic application info
        public Guid ApplicantContactId { get; set; }
        public ContactResponse? ApplicantSummary { get; set; }
        public Guid PropertyId { get; set; }
        public PropertyResponse? PropertySummary { get; set; }
        
        public string BuildingPurpose { get; set; } = default!;
        public int NoOfFloors { get; set; }
        public string? ArchitectName { get; set; }
        public string? EngineerName { get; set; }
        public string? Remarks { get; set; }
        
        public DateTime SubmittedOn { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string? ApprovedByUserId { get; set; }
        
        // Workflow-specific reports
        public string? PlanningReport { get; set; }
        public string? EngineerReport { get; set; }
        public string? CommissionerDecision { get; set; }

        public Guid? AssignmentId { get; set; }

        public Guid? SiteInspectionId { get; set; }

        public Guid? PlanningCommitteeReviewId { get; set; }

        // Related data
        public List<ApplicationDocumentResponse> Documents { get; set; } = new();
        public List<WorkflowLogResponse> WorkflowHistory { get; set; } = new();
    }
}