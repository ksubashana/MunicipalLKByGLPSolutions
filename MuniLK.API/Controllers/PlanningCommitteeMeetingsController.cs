using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.PlanningCommitteeMeetings.Commands;
using MuniLK.Application.PlanningCommitteeMeetings.DTOs;
using MuniLK.Application.PlanningCommitteeMeetings.Queries;
using MuniLK.Application.Generic.Result;

namespace MuniLK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "SuperAdmin,Admin,Officer")] // restrict management to officers/admins
    public class PlanningCommitteeMeetingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PlanningCommitteeMeetingsController(IMediator mediator) => _mediator = mediator;

        [HttpGet("GetMeetings")]
        public async Task<ActionResult<List<PlanningCommitteeMeetingResponse>>> GetMeetings([FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null)
        {
            var list = await _mediator.Send(new GetPlanningCommitteeMeetingsQuery(start, end));
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult<Result<PlanningCommitteeMeetingResponse>>> Create([FromBody] PlanningCommitteeMeetingRequest request)
        {
            var userId = User.Identity?.Name ?? User.FindFirst("sub")?.Value ?? "unknown-user";
            var result = await _mediator.Send(new CreatePlanningCommitteeMeetingCommand(request, userId));
            if (!result.Succeeded) return BadRequest(result.Error);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Result<PlanningCommitteeMeetingResponse>>> Update(Guid id, [FromBody] PlanningCommitteeMeetingRequest request)
        {
            if (request.Id.HasValue && request.Id.Value != id) return BadRequest("Id mismatch");
            var userId = User.Identity?.Name ?? User.FindFirst("sub")?.Value ?? "unknown-user";
            var result = await _mediator.Send(new UpdatePlanningCommitteeMeetingCommand(id, request, userId));
            if (!result.Succeeded) return BadRequest(result.Error);
            return Ok(result);
        }

        [HttpPost("{id:guid}/assign-application")]
        public async Task<ActionResult<Result>> AssignApplication(Guid id, [FromBody] Guid applicationId)
        {
            var userId = User.Identity?.Name ?? User.FindFirst("sub")?.Value ?? "unknown-user";
            var result = await _mediator.Send(new AssignApplicationToMeetingCommand(id, applicationId, userId));
            if (!result.Succeeded) return BadRequest(result.Error);
            return Ok(result);
        }

        [HttpPost("{id:guid}/record-outcome")]
        public async Task<ActionResult<Result>> RecordOutcome(Guid id, [FromBody] RecordOutcomeRequest body)
        {
            var userId = User.Identity?.Name ?? User.FindFirst("sub")?.Value ?? "unknown-user";
            var result = await _mediator.Send(new RecordMeetingOutcomeCommand(id, body.Decision, body.Notes, userId));
            if (!result.Succeeded) return BadRequest(result.Error);
            return Ok(result);
        }
    }

    public class RecordOutcomeRequest
    {
        public MuniLK.Domain.Constants.Flows.CommitteeDecision Decision { get; set; }
        public string? Notes { get; set; }
    }
}
