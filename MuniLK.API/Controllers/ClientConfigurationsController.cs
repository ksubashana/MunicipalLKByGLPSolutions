using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.Generic.DTOs;
using MuniLK.Application.Generic.Interfaces;

namespace MuniLK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientConfigurationController : ControllerBase
    {
        private readonly IClientConfigurationService _service;

        public ClientConfigurationController(IClientConfigurationService service)
        {
            _service = service;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ClientConfigurationCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), result);

            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ClientConfigurationUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.UpdateAsync(dto);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
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
