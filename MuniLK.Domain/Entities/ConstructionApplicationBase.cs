using MuniLK.Domain.Constants.Flows;
using MuniLK.Domain.Entities.ContactEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Domain.Entities
{
    public abstract class ConstructionApplicationBase
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid ApplicantContactId { get; set; }
        public Contact ApplicantContact { get; set; } = default!;

        public Guid PropertyId { get; set; }
        public Property Property { get; set; } = default!;

        public DateTime SubmittedOn { get; set; }
        public string? ApplicationNumber { get; set; } // e.g. BP/2025/000123
    }

    public class BuildingPlanApplication : ConstructionApplicationBase
    {
        public string BuildingPurpose { get; set; } = default!; // Residential / Commercial / Mixed etc.
        public int NoOfFloors { get; set; }
        public string? ArchitectName { get; set; }
        public string? EngineerName { get; set; }
        public string? Remarks { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string? ApprovedByUserId { get; set; }
        
        // Workflow status using BuildingAndPlanSteps instead of BuildingPlanStatus
        public BuildingAndPlanSteps Status { get; set; }
        
        // Workflow-specific report fields
        public string? PlanningReport { get; set; }
        public string? EngineerReport { get; set; }
        public string? CommissionerDecision { get; set; }

        // Workflow child entity foreign keys
        public Guid? AssignmentId { get; set; }
        public Assignment? Assignment { get; set; }

        public Guid? SiteInspectionId { get; set; }
        public SiteInspection? SiteInspection { get; set; }

        public Guid? PlanningCommitteeReviewId { get; set; }
        public PlanningCommitteeReview? PlanningCommitteeReview { get; set; }

        // Navigation properties
        public ICollection<DocumentLink> Documents { get; set; } = new List<DocumentLink>();
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public ICollection<WorkflowLog> WorkflowLogs { get; set; } = new List<WorkflowLog>();
    }
}
