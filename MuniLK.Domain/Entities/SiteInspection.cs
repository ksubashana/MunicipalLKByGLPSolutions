using MuniLK.Domain.Constants.Flows;
using MuniLK.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MuniLK.Domain.Entities
{
    public class SiteInspection : IHasTenant
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }

        // Basic References
        public Guid ApplicationId { get; set; }
        public Guid? InspectionId { get; set; } // Reference to existing assignment table - If possible change this is AssignmentId for better naming consistency 

        // Core Outcome (Extended)
        [Required]
        public InspectionStatus Status { get; set; }
        public string? Remarks { get; set; }

        // 1. Inspection Metadata
        [Required]
        public DateTime InspectionDate { get; set; }
        public string? OfficersPresent { get; set; } // Name + Designation
        public string? GpsCoordinates { get; set; }
        public string? PhotosPaths { get; set; } // JSON serialized list of file paths

        // 2. Site Conditions Verification (Yes/No + Notes)
        public bool? AccessRoadWidthCondition { get; set; }
        public string? AccessRoadWidthNotes { get; set; }
        
        public bool? BoundaryVerification { get; set; }
        public string? BoundaryVerificationNotes { get; set; }
        
        public bool? Topography { get; set; }
        public string? TopographyNotes { get; set; }
        
        public bool? ExistingStructures { get; set; }
        public string? ExistingStructuresNotes { get; set; }
        
        public bool? EncroachmentsReservations { get; set; }
        public string? EncroachmentsReservationsNotes { get; set; }

        // 3. Compliance Checks (Yes/No + Notes)
        public bool? MatchesSurveyPlan { get; set; }
        public string? MatchesSurveyPlanNotes { get; set; }
        
        public bool? ZoningCompatible { get; set; }
        public string? ZoningCompatibleNotes { get; set; }
        
        public bool? SetbacksObserved { get; set; }
        public string? SetbacksObservedNotes { get; set; }
        
        // Detailed setbacks (optional structured data)
        public bool? FrontSetback { get; set; }
        public bool? RearSetback { get; set; }
        public bool? SideSetbacks { get; set; }
        
        public bool? EnvironmentalConcerns { get; set; }
        public string? EnvironmentalConcernsNotes { get; set; }

        // 4. Decision Support
        public string? RequiredModifications { get; set; }
        public string? ClearancesRequired { get; set; } // JSON serialized list of ClearanceType
        
        [Required]
        public FinalRecommendation FinalRecommendation { get; set; }

        // Timestamps
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        // Navigation Properties
        public BuildingPlanApplication? Application { get; set; }
        public ICollection<EntityOptionSelection>? OptionSelections { get; set; }
    }
}