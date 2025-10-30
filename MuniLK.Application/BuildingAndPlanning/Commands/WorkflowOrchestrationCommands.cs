using MediatR;
using MuniLK.Application.Generic.Result;
using System;

namespace MuniLK.Application.BuildingAndPlanning.Commands
{
    /// <summary>
    /// Command to assign an inspector to a building plan application
    /// </summary>
    public record AssignInspectionWorkflowCommand(
        Guid BuildingPlanApplicationId,
        Guid AssignmentId,
        string? Remarks,
        string? AssignedUserId = null) : IRequest<Result>;

    /// <summary>
    /// Command to link a site inspection to a building plan application
    /// </summary>
    public record CreateSiteInspectionWorkflowCommand(
        Guid BuildingPlanApplicationId,
        Guid SiteInspectionId,
        string? Remarks,
        string? AssignedUserId = null) : IRequest<Result>;

    /// <summary>
    /// Command to link a planning committee review to a building plan application
    /// </summary>
    public record CreatePlanningCommitteeReviewWorkflowCommand(
        Guid BuildingPlanApplicationId,
        Guid PlanningCommitteeReviewId,
        string? Remarks,
        string? AssignedUserId = null) : IRequest<Result>;

    /// <summary>
    /// Command to assign a building plan application to a committee for review
    /// </summary>
    public record AssignToCommitteeWorkflowCommand(
        Guid BuildingPlanApplicationId,
        Guid PlanningCommitteeReviewId,
        DateTime MeetingDate,
        string? Remarks,
        string? AssignedUserId = null) : IRequest<Result>;
}
