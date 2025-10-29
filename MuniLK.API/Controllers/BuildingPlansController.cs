using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.BuildingAndPlanning.Commands;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.BuildingAndPlanning.Queries;
using Syncfusion.Blazor;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using MuniLK.Domain.Constants;

namespace MuniLK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingPlansController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BuildingPlansController(IMediator mediator) => _mediator = mediator;

        // POST: /api/building-plans
        [HttpPost("Submit")]
        [Authorize(Policy = "SubmitBuildingPlan")]
        public async Task<IActionResult> Submit([FromBody] SubmitBuildingPlanRequest request, CancellationToken ct)
        {
            var id = await _mediator.Send(new SubmitBuildingPlanCommand(request));
            return id.Succeeded ? Ok(id.Data) : BadRequest(id.Error);
        }

        // New: summary endpoint used by the Blazor page
        [HttpGet("{id:guid}/summary")]
        [Authorize]
        public async Task<IActionResult> GetSummary(Guid id)
        {
            var dto = await _mediator.Send(new GetBuildingPlanSummaryQuery(id));
            return dto is null ? NotFound() : Ok(dto);
        }

        // New workflow snapshot endpoint
        [HttpGet("{id:guid}/workflow-snapshot")]
        [Authorize]
        public async Task<IActionResult> GetWorkflowSnapshot(Guid id)
        {
            var snapshot = await _mediator.Send(new GetBuildingPlanWorkflowSnapshotQuery(id));
            return snapshot is null ? NotFound() : Ok(snapshot);
        }

        // New: workflow history (logs) endpoint
        [HttpGet("{id:guid}/workflow-history")]
        [Authorize]
        public async Task<IActionResult> GetWorkflowHistory(Guid id)
        {
            var result = await _mediator.Send(new GetWorkflowHistoryQuery(id));
            if (!result.Succeeded) return NotFound(result.Error);
            // Map to simplified DTO used by Blazor component
            var logs = result.Data.Select(l => new WorkflowLogResponse
            {
                From = l.PreviousStatus ?? string.Empty,
                To = l.NewStatus,
                Action = l.ActionTaken,
                Remarks = l.Remarks,
                PerformedByUserId = l.PerformedByDisplayName,
                PerformedOn = l.PerformedAt
            }).OrderByDescending(x => x.PerformedOn).ToList();
            return Ok(logs);
        }

        // New: search endpoint for Syncfusion DataGrid
        [HttpGet("search")]
        public async Task<object> SearchBuildingPlans()
        {
            List<BuildingPlanApplicationDto> data =  await _mediator.Send(new SearchBuildingPlansQuery());
            return new { Items = data, Count = data.Count() };

            //return data; // returns List<BuildingPlanResponse>
        }

        // New: paginated search endpoint
        [HttpGet("search-list")]
        public async Task<IActionResult> SearchList([FromQuery] int skip = 0, [FromQuery] int take = 20, [FromQuery] string? search = null)
        {
            Guid tenantId = Guid.Parse(Request.Headers["TenantId"].FirstOrDefault());

            var (items, total) = await _mediator.Send(new SearchBuildingPlanListQuery(tenantId,skip, take, search));
            return Ok(new { Items = items, Count = total });
        }

        // POST: /api/building-plans/{id}/verify
        [HttpPost("{id:guid}/verify")]
        [Authorize(Roles = Roles.Officer)]
        public async Task<IActionResult> Verify(Guid id, [FromBody] string? remarks)
        {
            var result = await _mediator.Send(new VerifyBuildingPlanCommand(id, remarks));
            return result.Succeeded ? Ok() : BadRequest(result.Error);
        }


        // POST: /api/building-plans/{id}/engineer-approve
        [HttpPost("{id:guid}/engineer-approve")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin},{Roles.Officer}")]
        public async Task<IActionResult> EngineerApprove(Guid id, [FromBody] string? remarks)
        {
            var result = await _mediator.Send(new EngineerApproveBuildingPlanCommand(id, remarks));
            return result.Succeeded ? Ok() : BadRequest(result.Error);
        }

        // POST: /api/building-plans/{id}/final-approve
        [HttpPost("{id:guid}/final-approve")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin},{Roles.Officer}")]
        public async Task<IActionResult> FinalApprove(Guid id, [FromBody] string? remarks)
        {
            var result = await _mediator.Send(new FinalApproveBuildingPlanCommand(id, remarks));
            return result.Succeeded ? Ok() : BadRequest(result.Error);
        }

        // POST: /api/building-plans/{id}/reject
        [HttpPost("{id:guid}/reject")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin},{Roles.Officer}")] // decide
        public async Task<IActionResult> Reject(Guid id, [FromBody] RejectDto dto)
        {
            var result = await _mediator.Send(new RejectBuildingPlanCommand(id, dto.Reason));
            return result.Succeeded ? Ok() : BadRequest(result.Error);
        }
        [HttpPost("{id:guid}/committee-review-test")]
        [AllowAnonymous] // Temporary for testing
        public IActionResult TestCommitteeReview(Guid id, [FromBody] object request)
        {
            return Ok(new { Id = id, RequestType = request?.GetType()?.Name, Body = request });
        }
        // POST: /api/building-plans/{id}/committee-review
        [HttpPost("{id:guid}/committee-review")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin},{Roles.Officer}")]
        public async Task<IActionResult> SaveCommitteeReview(
            Guid id,
            [FromBody] PlanningCommitteeReviewRequest request,
            [FromQuery] bool finalize = false)
        {
            // Debug model state
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .Select(x => new { Field = x.Key, Errors = x.Value?.Errors.Select(e => e.ErrorMessage) })
                    .ToArray();
                
                return BadRequest(new { Errors = errors, Message = "Validation failed" });
            }
            
            if (request is null) return BadRequest("Request body required.");
            if (id != request.ApplicationId) return BadRequest("Route id and request.ApplicationId mismatch.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                         ?? User.FindFirst("sub")?.Value 
                         ?? "unknown-user";

            var cmd = new SavePlanningCommitteeReviewCommand
            {
                Request = request,
                UserId = userId
            };

            var result = await _mediator.Send(cmd);
            if (!result.Succeeded)
                return BadRequest(result.Error);

            // OPTIONAL: advance workflow based on decision (uncomment if desired)
            // switch (request.CommitteeDecision)
            // {
            //     case CommitteeDecision.Approve:
            //     case CommitteeDecision.ApproveWithConditions:
            //         await _mediator.Send(new AdvanceBuildingPlanWorkflowCommand(id, ReviewDecision.Approved, userId, "CommitteeDecision"));
            //         break;
            //     case CommitteeDecision.Reject:
            //         await _mediator.Send(new AdvanceBuildingPlanWorkflowCommand(id, ReviewDecision.Rejected, userId, "CommitteeDecision"));
            //         break;
            //     case CommitteeDecision.DeferForClarifications:
            //         await _mediator.Send(new AdvanceBuildingPlanWorkflowCommand(id, ReviewDecision.ClarificationRequired, userId, "CommitteeDecision"));
            //         break;
            // }

            return Ok(new
            {
                Draft = !finalize,
                Finalized = finalize,
                Review = result.Data
            });
        }
    }

    public record AssignInspectionDto(DateTime ScheduledOn, string InspectorUserId, string? Remarks);
    public record CompleteInspectionDto(string Report);
    public record RejectDto(string Reason);
}
