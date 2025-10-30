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
using MuniLK.Domain.Entities;
using MuniLK.Domain.Constants.Flows;

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
                PerformedByUserId = l.PerformedByDisplayName ?? l.PerformedByUserId,
                PerformedOn = l.PerformedAt
            }).ToList();
            return Ok(logs);
        }

        // New: search endpoint for Syncfusion DataGrid
        [HttpGet("search")]
        public async Task<object> SearchBuildingPlans()
        {
            var data = await _mediator.Send(new SearchBuildingPlansQuery());
            return new { Items = data, Count = data.Count() };
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
        public async Task<IActionResult> SaveCommitteeReview(Guid id, [FromBody] PlanningCommitteeReviewRequest request, [FromQuery] bool finalize = false)
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

            return Ok(new
            {
                Draft = !finalize,
                Finalized = finalize,
                Review = result.Data
            });
        }

        // New: schedule assignment to planning committee (creates placeholder review & sets status AssignToCommittee)
        [HttpPost("{id:guid}/assign-to-committee")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin},{Roles.Officer}")]
        public async Task<IActionResult> AssignToCommittee(Guid id, [FromBody] AssignToCommitteeRequest request, CancellationToken ct)
        {
            if (id != request.ApplicationId) return BadRequest("Route id mismatch.");
            if (request.MeetingDate.Date < DateTime.Today) return BadRequest("Meeting date cannot be in the past.");
            // Create placeholder PlanningCommitteeReview entity externally via SavePlanningCommitteeReviewCommand with Pending decision
            var placeholder = new PlanningCommitteeReviewRequest
            {
                ApplicationId = request.ApplicationId,
                MeetingDate = request.MeetingDate,
                CommitteeType = request.CommitteeType,
                MeetingReferenceNo = request.MeetingReferenceNo,
                ChairpersonName = request.ChairpersonName,
                MembersPresent = request.MembersPresent,
                RecordedByOfficer = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "unknown-user",
                CommitteeDecision = CommitteeDecision.Pending
            };
            // Save placeholder (will create new review if not exists)
            var saveResult = await _mediator.Send(new SavePlanningCommitteeReviewCommand { Request = placeholder, UserId = placeholder.RecordedByOfficer });
            if (!saveResult.Succeeded) return BadRequest(saveResult.Error);
            // Orchestrate workflow status change to AssignToCommittee
            var workflowResult = await _mediator.Send(new AssignToCommitteeWorkflowCommand(id, saveResult.Data.Id, request.MeetingDate, request.Remarks, null));
            if (!workflowResult.Succeeded) return BadRequest(workflowResult.Error);
            return Ok(new { ReviewId = saveResult.Data.Id });
        }

        // New: update committee review result (decision & outcomes)
        [HttpPut("{id:guid}/committee-review-result")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin},{Roles.Officer}")]
        public async Task<IActionResult> UpdateCommitteeReview(Guid id, [FromBody] PlanningCommitteeReviewRequest request, CancellationToken ct)
        {
            if (id != request.ApplicationId) return BadRequest("Route id mismatch.");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "unknown-user";
            request.RecordedByOfficer = userId; // ensure audit
            var result = await _mediator.Send(new SavePlanningCommitteeReviewCommand { Request = request, UserId = userId });
            return result.Succeeded ? Ok(result.Data) : BadRequest(result.Error);
        }
    }

    public record AssignInspectionDto(DateTime ScheduledOn, string InspectorUserId, string? Remarks);
    public record CompleteInspectionDto(string Report);
    public record RejectDto(string Reason);
    public record AssignToCommitteeRequest(Guid ApplicationId, DateTime MeetingDate, CommitteeType CommitteeType, string MeetingReferenceNo, string ChairpersonName, List<CommitteeMember> MembersPresent, string? Remarks);
}
