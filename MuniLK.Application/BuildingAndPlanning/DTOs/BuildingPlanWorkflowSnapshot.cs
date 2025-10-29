using MuniLK.Domain.Constants.Flows;

namespace MuniLK.Application.BuildingAndPlanning.DTOs
{
    /// <summary>
    /// Lightweight workflow snapshot for a Building Plan Application used for UI step routing.
    /// </summary>
    public class BuildingPlanWorkflowSnapshot
    {
        public Guid ApplicationId { get; set; }
        public string ApplicationNumber { get; set; } = string.Empty;
        public BuildingAndPlanSteps CurrentStatus { get; set; }
        public InspectionStatus? SiteInspectionStatus { get; set; }
        public bool HasAssignment { get; set; }
        public DateTime? AssignmentDate { get; set; }
        public bool IsAssignmentExpired { get; set; }
        public bool IsInspectionCompleted { get; set; }
        public bool CanProceedToCommittee { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string? NextStage { get; set; }
    }
}
