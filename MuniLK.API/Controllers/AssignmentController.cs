using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.Assignments.Commands;
using MuniLK.Application.Assignments.DTOs;
using MuniLK.Application.Assignments.Queries;
using MuniLK.Application.BuildingAndPlanning.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MuniLK.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AssignmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AssignmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAssignment([FromBody] CreateAssignmentRequest request)
        {
            var assignmentId = await _mediator.Send(new CreateAssignmentCommand(request));
            // Then, orchestrate the workflow to link it to the BuildingPlanApplication
            var result = await _mediator.Send(new AssignInspectionWorkflowCommand(
                BuildingPlanApplicationId: request.EntityId,
                AssignmentId: assignmentId,
                Remarks: "Inspector assigned for site verification",
                AssignedUserId: request.AssignedToUser
            ));
            return CreatedAtAction(nameof(GetAssignmentById), new { id = assignmentId }, assignmentId);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAssignmentById(Guid id)
        {
            var result = await _mediator.Send(new GetAssignmentByIdQuery(id));
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpGet("by-entity")]
        public async Task<IActionResult> GetByEntity([FromQuery] Guid moduleId, [FromQuery] Guid entityId)
        {
            var list = await _mediator.Send(new GetAssignmentsQuery(moduleId, entityId));
            var latest = list.OrderByDescending(a => a.CompletedAt ?? a.AssignmentDate).FirstOrDefault();
            return latest is null ? NotFound() : Ok(latest);
        }

        [HttpPost("{id:guid}/complete")]
        public async Task<IActionResult> Complete(Guid id, [FromBody] CompleteAssignmentRequest request)
        {
            if (request is null) return BadRequest("Request body is required.");
            if (id != request.AssignmentId) return BadRequest("Route id and body AssignmentId mismatch.");

            var ok = await _mediator.Send(new CompleteAssignmentCommand(request));
            return ok ? Ok() : NotFound();
        }
    }
}
