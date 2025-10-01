
    using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.PropertiesLK.DTOs;
using MuniLK.Application.PropertyOwners.Commands.CreatePropertyOwner;
using MuniLK.Application.PropertyOwners.DTOs;
using MuniLK.Application.PropertyOwners.Queries.GetOwners;


namespace MuniLK.Api.Controllers
    {
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyOwnersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertyOwnersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Add a new property owner
        /// </summary>
        [HttpPost("Create")]
        public async Task<ActionResult<PropertyOwnerResponse>> Create([FromBody] CreatePropertyOwnerRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request");
            var command = new CreatePropertyOwnerCommand(request);

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Get all owners of a specific property
        /// </summary>
        [HttpGet("GetByProperty")]
        [Authorize]

        public async Task<ActionResult<List<PropertyOwnerResponse>>> GetByProperty(Guid propertyId)
        {
            var result = await _mediator.Send(new GetOwnersByPropertyIdQuery(propertyId));
            return Ok(result);
        }

        /// <summary>
        /// Get a specific property owner by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PropertyOwnerResponse>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetOwnersByPropertyIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Update ownership details (e.g., OwnershipType)
        /// </summary>
        //[HttpPut("{id:guid}")]
        //public async Task<ActionResult<PropertyOwnerResponse>> Update(Guid id, [FromBody] UpdatePropertyOwnerCommand command)
        //{
        //    if (id != command.Id)
        //        return BadRequest("ID mismatch");

        //    var result = await _mediator.Send(command);
        //    if (result == null)
        //        return NotFound();

        //    return Ok(result);
        //}

        ///// <summary>
        ///// Delete a property owner record
        ///// </summary>
        //[HttpDelete("{id:guid}")]
        //public async Task<ActionResult> Delete(Guid id)
        //{
        //    var success = await _mediator.Send(new DeletePropertyOwnerCommand(id));
        //    if (!success)
        //        return NotFound();

        //    return NoContent();
        //}
    }
    }


