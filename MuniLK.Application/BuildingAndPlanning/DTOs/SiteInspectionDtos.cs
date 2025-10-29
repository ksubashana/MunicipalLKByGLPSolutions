using MuniLK.Domain.Constants.Flows;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MuniLK.Application.BuildingAndPlanning.DTOs
{
    public class SiteInspectionRequest
    {
        [Required]
        public Guid ApplicationId { get; set; }
        
        [Required]
        public Guid? InspectionId { get; set; }
        
        // Core Outcome (Extended)
        [Required]
        public InspectionStatus Status { get; set; }
        public string? Remarks { get; set; }
        public string? PropertyAddress { get; set; }

        // 1. Inspection Metadata
        [Required]
        public DateTime InspectionDate { get; set; }
        public string? InspectionAssignedTo { get; set; }
        public string? GpsCoordinates { get; set; }
        public List<IFormFile>? Photos { get; set; }

        // 2. Site Conditions Verification
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

        // 3. Compliance Checks
        public bool? MatchesSurveyPlan { get; set; }
        public string? MatchesSurveyPlanNotes { get; set; }
        
        public bool? ZoningCompatible { get; set; }
        public string? ZoningCompatibleNotes { get; set; }
        
        public bool? SetbacksObserved { get; set; }
        public string? SetbacksObservedNotes { get; set; }
        
        public bool? FrontSetback { get; set; }
        public bool? RearSetback { get; set; }
        public bool? SideSetbacks { get; set; }
        
        public bool? EnvironmentalConcerns { get; set; }
        public string? EnvironmentalConcernsNotes { get; set; }

        // 4. Decision Support
        public string? RequiredModifications { get; set; }
        // Deprecated: use Option Selections via EntityOptionSelection table (List<Guid> OptionItemIds)
        public List<Guid>? ClearanceOptionItemIds { get; set; }
        
        [Required]
        public FinalRecommendation FinalRecommendation { get; set; }
    }

    public class SiteInspectionResponse
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid? InspectionId { get; set; }
        
        // Core Outcome
        public InspectionStatus Status { get; set; }
        public string? Remarks { get; set; }

        // Inspection Metadata
        public DateTime InspectionDate { get; set; }
        public string? OfficersPresent { get; set; }
        public string? GpsCoordinates { get; set; }
        public List<string>? PhotoUrls { get; set; }

        // Site Conditions - Simplified for response
        public List<SiteConditionResult> SiteConditions { get; set; } = new();
        public List<ComplianceCheckResult> ComplianceChecks { get; set; } = new();
        
        // Decision Support
        public string? RequiredModifications { get; set; }
        // Deprecated legacy enum list; now selections resolved externally
        public List<Guid>? ClearanceOptionItemIds { get; set; }
        public FinalRecommendation FinalRecommendation { get; set; }
        
        // Timestamps
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }

    public class SiteConditionResult
    {
        public string Name { get; set; } = default!;
        public bool? Result { get; set; }
        public string? Notes { get; set; }
    }

    public class ComplianceCheckResult
    {
        public string Name { get; set; } = default!;
        public bool? Result { get; set; }
        public string? Notes { get; set; }
    }

    // Extended CompleteInspectionDto to maintain backward compatibility
    public class CompleteInspectionDtoExtended
    {
        // Original field for backward compatibility
        public string? Report { get; set; }
        
        // New structured inspection data
        public SiteInspectionRequest? SiteInspection { get; set; }
    }
}