using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.BuildingAndPlanning.Commands;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.BuildingAndPlanning.Queries;
using MuniLK.Domain.Constants;

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
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin},{Roles.Officer}")] // adjust roles
        public async Task<IActionResult> AssignInspection(Guid id, [FromBody] AssignInspectionDto dto)
        {
            var result = await _mediator.Send(new AssignInspectionCommand(id, dto.ScheduledOn, dto.InspectorUserId, dto.Remarks));
            return result.Succeeded ? Ok() : BadRequest(result.Error);
        }

        // POST: /api/building-plans/{id}/complete-site-inspection/{inspectionId}
        [HttpPost("{id:guid}/complete-site-inspection")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin},{Roles.Inspector}")]
        public async Task<IActionResult> CompleteSiteInspection(Guid id, [FromBody] SiteInspectionRequest request)
        {
            var result = await _mediator.Send(new CompleteSiteInspectionCommand(id, request));
            return result.Succeeded ? Ok() : BadRequest(result.Error);
        }

        // GET: /api/Inspection/{inspectionId}
        [HttpGet("{inspectionId:guid}")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin},{Roles.Officer}")]
        public async Task<IActionResult> GetSiteInspection(Guid inspectionId)
        {
            var result = await _mediator.Send(new GetSiteInspectionQuery(inspectionId));
            return result != null ? Ok(result) : NotFound();
        }
    }
}
