using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.Generic.DTOs;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Domain.Entities;

namespace MuniLK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientConfigurationsController : ControllerBase
    {
        private readonly IClientConfigurationService _service;

        public ClientConfigurationsController(IClientConfigurationService service)
        {
            _service = service;
        }

        // Create returns the newly created configuration and sets Location header pointing to GetById
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ClientConfigurationCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // CHANGE: Expect CreateAsync to return ClientConfigurationDto (not string)
                ClientConfiguration created = await _service.CreateAsync(dto); // adjust service signature accordingly

                if (created is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Creation failed.");

                if (created.Id == Guid.Empty)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Created entity has empty Id.");

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ClientConfigurationUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _service.UpdateAsync(dto);
                if (updated == null)
                    return NotFound();

                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // CHANGE: Id type switched from int to Guid (matches DTO.Id)
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var config = await _service.GetByIdAsync(id);
            if (config == null)
                return NotFound();

            return Ok(config);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var configs = await _service.GetAllAsync();
            return Ok(configs);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
