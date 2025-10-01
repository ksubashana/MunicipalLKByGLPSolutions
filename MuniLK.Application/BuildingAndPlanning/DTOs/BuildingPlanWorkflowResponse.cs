using MuniLK.Domain.Constants.Flows;

namespace MuniLK.Application.BuildingAndPlanning.DTOs
{
    /// <summary>
    /// Response DTO for workflow advancement operations
    /// </summary>
    public class BuildingPlanWorkflowResponse
    {
        public Guid ApplicationId { get; set; }
        public BuildingAndPlanSteps PreviousStatus { get; set; }
        public BuildingAndPlanSteps NewStatus { get; set; }
        public string ActionTaken { get; set; } = default!;
        public DateTime ProcessedAt { get; set; }
        public string? Comments { get; set; }
    }
}