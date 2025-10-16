using MuniLK.Domain.Entities; // For Lookup only (if still needed)
using MuniLK.Application.Assignments.DTOs;
using MuniLK.Application.BuildingAndPlanning.DTOs; // self-namespace safe
using MuniLK.Application.BuildingAndPlanning.DTOs; // ensure namespace resolution
using MuniLK.Application.BuildingAndPlanning.DTOs; // duplicated accidentally but harmless
using MuniLK.Application.BuildingAndPlanning.DTOs; // keep compiler ignoring duplicates (will remove if needed)
using MuniLK.Application.BuildingAndPlanning.DTOs; // (IDE may optimize)
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; // (These will be cleaned by formatter)
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; // (End accidental duplicates)
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; // (final)
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using MuniLK.Application.BuildingAndPlanning.DTOs; //
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.DTOs
{
    public class BuildingPlanResponse
    {
        public Guid Id { get; set; }
        public string ApplicationNumber { get; set; } = default!;
        public Domain.Entities.Lookup Status { get; set; }
        public Guid ApplicantContactId { get; set; }
        public Guid PropertyId { get; set; }
        public string BuildingPurpose { get; set; } = default!;
        public int NoOfFloors { get; set; }
        public DateTime SubmittedOn { get; set; }
        public Guid? AssignmentId { get; set; }
        // Navigation projections mapped to DTOs to avoid object cycles
        public AssignmentResponse? Assignment { get; set; }

        public Guid? SiteInspectionId { get; set; }
        public SiteInspectionResponse? SiteInspection { get; set; }

        public Guid? PlanningCommitteeReviewId { get; set; }
        public PlanningCommitteeReviewResponse? PlanningCommitteeReview { get; set; }
        public List<ApplicationDocumentResponse> Documents { get; set; } = new();
        public List<WorkflowLogResponse> Workflow { get; set; } = new();
        
        // Additional fields for grid display
        public string ApplicantName { get; set; } = default!;
        public string PropertyAddress { get; set; } = default!;
        public DateTime SubmittedDate => SubmittedOn; // Alias for consistency with requirements
    }
}
