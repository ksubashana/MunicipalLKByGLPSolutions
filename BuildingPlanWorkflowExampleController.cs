using Microsoft.AspNetCore.Mvc;
using MediatR;
using MuniLK.Application.BuildingAndPlanning.Commands;
using MuniLK.Application.Assignments.Commands; // Hypothetical
using MuniLK.Application.Generic.Result;
using System;
using System.Threading.Tasks;

namespace MuniLK.API.Controllers
{
    /// <summary>
    /// Example controller showing how to use workflow orchestration commands
    /// This is for reference only - adapt to your actual API structure
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingPlanWorkflowExampleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BuildingPlanWorkflowExampleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Example: Assign inspector to building plan application
        /// </summary>
        [HttpPost("assign-inspector")]
        public async Task<IActionResult> AssignInspector(
            [FromBody] AssignInspectorRequest request)
        {
            // Step 1: Create the Assignment entity using existing generic handler
            // (This would be your existing CreateAssignmentCommand)
            var assignmentResult = await CreateAssignmentAsync(
                request.BuildingPlanApplicationId,
                request.InspectorUserId,
                request.TaskType,
                request.DueDate);

            if (!assignmentResult.Succeeded)
                return BadRequest(assignmentResult.Message);

            // Step 2: Orchestrate the workflow to link assignment and update status
            var workflowResult = await _mediator.Send(new AssignInspectionWorkflowCommand(
                BuildingPlanApplicationId: request.BuildingPlanApplicationId,
                AssignmentId: assignmentResult.Value,
                Remarks: request.Remarks,
                AssignedUserId: request.InspectorUserId));

            if (!workflowResult.Succeeded)
                return BadRequest(workflowResult.Message);

            return Ok(new { Message = "Inspector assigned successfully", AssignmentId = assignmentResult.Value });
        }

        /// <summary>
        /// Example: Complete site inspection and update workflow
        /// </summary>
        [HttpPost("complete-site-inspection")]
        public async Task<IActionResult> CompleteSiteInspection(
            [FromBody] CompleteSiteInspectionRequest request)
        {
            // Step 1: Create the SiteInspection entity using existing handler
            var siteInspectionResult = await _mediator.Send(new CompleteSiteInspectionCommand(
                Id: request.BuildingPlanApplicationId,
                Request: request.SiteInspectionData));

            if (!siteInspectionResult.Succeeded)
                return BadRequest(siteInspectionResult.Message);

            // Step 2: Orchestrate the workflow to link site inspection and update status
            var workflowResult = await _mediator.Send(new CreateSiteInspectionWorkflowCommand(
                BuildingPlanApplicationId: request.BuildingPlanApplicationId,
                SiteInspectionId: request.SiteInspectionId, // You would get this from the result
                Remarks: "Site inspection completed - moving to review stage",
                AssignedUserId: request.ReviewerUserId));

            if (!workflowResult.Succeeded)
                return BadRequest(workflowResult.Message);

            return Ok(new { Message = "Site inspection completed successfully" });
        }

        /// <summary>
        /// Example: Schedule planning committee review
        /// </summary>
        [HttpPost("schedule-committee-review")]
        public async Task<IActionResult> ScheduleCommitteeReview(
            [FromBody] ScheduleCommitteeReviewRequest request)
        {
            // Step 1: Create the PlanningCommitteeReview entity using existing handler
            var reviewResult = await _mediator.Send(new SavePlanningCommitteeReviewCommand
            {
                ApplicationId = request.BuildingPlanApplicationId,
                MeetingDate = request.MeetingDate,
                CommitteeType = request.CommitteeType,
                MeetingReferenceNo = request.MeetingReferenceNo,
                ChairpersonName = request.ChairpersonName,
                MembersPresent = request.MembersPresent,
                CommitteeDecision = request.CommitteeDecision,
                RecordedByOfficer = request.RecordedByOfficer
            });

            if (!reviewResult.Succeeded)
                return BadRequest(reviewResult.Message);

            // Step 2: Orchestrate the workflow to link review and update status
            var workflowResult = await _mediator.Send(new CreatePlanningCommitteeReviewWorkflowCommand(
                BuildingPlanApplicationId: request.BuildingPlanApplicationId,
                PlanningCommitteeReviewId: reviewResult.Value,
                Remarks: $"Planning committee review scheduled for {request.MeetingDate:yyyy-MM-dd}",
                AssignedUserId: null)); // No specific user assignment for committee review

            if (!workflowResult.Succeeded)
                return BadRequest(workflowResult.Message);

            return Ok(new 
            { 
                Message = "Planning committee review scheduled successfully", 
                ReviewId = reviewResult.Value 
            });
        }

        // Helper method - replace with actual implementation
        private async Task<Result<Guid>> CreateAssignmentAsync(
            Guid applicationId, 
            string userId, 
            string taskType, 
            DateTime dueDate)
        {
            // Your existing Assignment creation logic
            // This is a placeholder - implement according to your actual Assignment creation command
            throw new NotImplementedException("Implement with your CreateAssignmentCommand");
        }
    }

    #region Request DTOs (for example only)

    public class AssignInspectorRequest
    {
        public Guid BuildingPlanApplicationId { get; set; }
        public string InspectorUserId { get; set; } = default!;
        public string TaskType { get; set; } = default!;
        public DateTime DueDate { get; set; }
        public string? Remarks { get; set; }
    }

    public class CompleteSiteInspectionRequest
    {
        public Guid BuildingPlanApplicationId { get; set; }
        public Guid SiteInspectionId { get; set; }
        public object SiteInspectionData { get; set; } = default!; // Replace with actual DTO
        public string? ReviewerUserId { get; set; }
    }

    public class ScheduleCommitteeReviewRequest
    {
        public Guid BuildingPlanApplicationId { get; set; }
        public DateTime MeetingDate { get; set; }
        public int CommitteeType { get; set; }
        public string MeetingReferenceNo { get; set; } = default!;
        public string ChairpersonName { get; set; } = default!;
        public string MembersPresent { get; set; } = default!;
        public int CommitteeDecision { get; set; }
        public string RecordedByOfficer { get; set; } = default!;
    }

    #endregion
}
