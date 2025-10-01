using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.Generic.Result;
using MuniLK.Domain.Constants.Flows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.Commands
{

    public record SubmitBuildingPlanCommand(SubmitBuildingPlanRequest Request) : IRequest<Result<string>>;
    public record VerifyBuildingPlanCommand(Guid Id, string? Remarks) : IRequest<Result>;
    public record AssignInspectionCommand(Guid Id, DateTime ScheduledOn, string InspectorUserId, string? Remarks) : IRequest<Result>;
    //public record CompleteInspectionCommand(Guid Id, string Report) : IRequest<Result>;
    public record CompleteSiteInspectionCommand(Guid Id, SiteInspectionRequest Request) : IRequest<Result>;
    public record EngineerApproveBuildingPlanCommand(Guid Id, string? Remarks) : IRequest<Result>;
    public record FinalApproveBuildingPlanCommand(Guid Id, string? Remarks) : IRequest<Result>;
    public record RejectBuildingPlanCommand(Guid Id, string Reason) : IRequest<Result>;
    
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
