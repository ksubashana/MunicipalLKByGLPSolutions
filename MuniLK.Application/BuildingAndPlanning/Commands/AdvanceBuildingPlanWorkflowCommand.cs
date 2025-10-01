using MediatR;
using MuniLK.Application.Generic.Result;
using MuniLK.Domain.Constants.Flows;

namespace MuniLK.Application.BuildingAndPlanning.Commands
{
    /// <summary>
    /// Command to advance a building plan application through its workflow stages
    /// </summary>
    public record AdvanceBuildingPlanWorkflowCommand(
        Guid ApplicationId,
        ReviewDecision Decision,
        string? Comments,
        Guid? AssignedToUserId,
        string PerformedByUserId,
        string PerformedByRole
    ) : IRequest<Result<BuildingPlanWorkflowResponse>>;
}