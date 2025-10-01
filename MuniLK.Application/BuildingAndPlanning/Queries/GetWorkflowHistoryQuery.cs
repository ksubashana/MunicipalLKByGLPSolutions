using MediatR;
using MuniLK.Application.Generic.Result;

namespace MuniLK.Application.BuildingAndPlanning.Queries
{
    /// <summary>
    /// Query to retrieve workflow history for a building plan application
    /// </summary>
    public record GetWorkflowHistoryQuery(Guid ApplicationId) : IRequest<Result<List<WorkflowHistoryResponse>>>;

    /// <summary>
    /// Response DTO for workflow history entries
    /// </summary>
    public class WorkflowHistoryResponse
    {
        public Guid Id { get; set; }
        public string ActionTaken { get; set; } = default!;
        public string? PreviousStatus { get; set; }
        public string NewStatus { get; set; } = default!;
        public string? Remarks { get; set; }
        public string PerformedByUserId { get; set; } = default!;
        public string? PerformedByRole { get; set; }
        public string? AssignedToUserId { get; set; }
        public bool IsSystemGenerated { get; set; }
        public DateTime PerformedAt { get; set; }
    }
}