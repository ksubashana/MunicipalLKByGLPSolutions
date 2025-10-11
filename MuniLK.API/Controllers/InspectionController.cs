using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.BuildingAndPlanning.Commands;
using MuniLK.Application.BuildingAndPlanning.DTOs;

namespace MuniLK.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class InspectionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InspectionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: /api/building-plans/{id}/assign-inspection
        [HttpPost("{id:guid}/assign-inspection")]
        [Authorize(Roles = "Clerk,Engineer")] // adjust roles
        public async Task<IActionResult> AssignInspection(Guid id, [FromBody] AssignInspectionDto dto)
        {
            var result = await _mediator.Send(new AssignInspectionCommand(id, dto.ScheduledOn, dto.InspectorUserId, dto.Remarks));
            return result.Succeeded ? Ok() : BadRequest(result.Error);
        }

        // POST: /api/building-plans/{id}/complete-inspection/{inspectionId}
        //[HttpPost("{id:guid}/complete-inspection/{inspectionId:guid}")]
        //[Authorize(Roles = "Inspector,Engineer,Admin")]
        //public async Task<IActionResult> CompleteInspection(Guid id, Guid inspectionId, [FromBody] CompleteInspectionDto dto)
        //{
        //    var result = await _mediator.Send(new CompleteInspectionCommand(id, dto.Report));
        //    return result.Succeeded ? Ok() : BadRequest(result.Error);
        //}

        // POST: /api/building-plans/{id}/complete-site-inspection/{inspectionId}
        [HttpPost("{id:guid}/complete-site-inspection")]
        [Authorize(Roles = "Inspector,Engineer,Admin")]
        public async Task<IActionResult> CompleteSiteInspection(Guid id, [FromBody] SiteInspectionRequest request)
        {
            var result = await _mediator.Send(new CompleteSiteInspectionCommand(id, request));
            return result.Succeeded ? Ok() : BadRequest(result.Error);
        }
    }
    }
    
