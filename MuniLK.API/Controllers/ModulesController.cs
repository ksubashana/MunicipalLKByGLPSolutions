using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.Generic.DTOs;
using MuniLK.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MuniLK.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ModulesController : ControllerBase
    {
        private readonly ModuleService _moduleService;

        public ModulesController(ModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> CreateModulesBulk([FromBody] List<ModuleCreateDto> createDtos)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdModules = new List<ModuleDto>();

            foreach (var dto in createDtos)
            {
                try
                {
                    var moduleDto = await _moduleService.CreateModuleAsync(dto);
                    createdModules.Add(moduleDto);
                }
                catch (InvalidOperationException ex)
                {
                    // Optionally: handle duplicates or other errors per item here
                    // For now, just skip or return error immediately
                    return Conflict($"Module with code '{dto.Code}' already exists.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error creating module '{dto.Code}': {ex.Message}");
                }
            }

            return Created("bulk", createdModules);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)] // For unique code violation
        public async Task<IActionResult> CreateModule([FromBody] ModuleCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var moduleDto = await _moduleService.CreateModuleAsync(createDto);
                return CreatedAtAction(nameof(GetModuleById), new { id = moduleDto.Id }, moduleDto);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); // For unique code violation
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating module: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)] // For unique code violation
        public async Task<IActionResult> UpdateModule(Guid id, [FromBody] ModuleUpdateDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest("Module ID in URL does not match ID in body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var moduleDto = await _moduleService.UpdateModuleAsync(updateDto);
                if (moduleDto == null)
                {
                    return NotFound($"Module with ID {id} not found.");
                }
                return Ok(moduleDto);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); // For unique code violation
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating module: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetModuleById(Guid id)
        {
            var moduleDto = await _moduleService.GetModuleByIdAsync(id);
            if (moduleDto == null)
            {
                return NotFound($"Module with ID {id} not found.");
            }
            return Ok(moduleDto);
        }
        [HttpGet("id-by-code/{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetModuleIdByCode(string code)
        {
            var moduleId = await _moduleService.GetModuleIdByCodeAsync(code);

            if (moduleId == null)
            {
                return NotFound($"Module with code '{code}' not found.");
            }

            return Ok(moduleId);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllModules()
        {
            var modules = await _moduleService.GetAllModulesAsync();
            return Ok(modules);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // For deletion constraints
        public async Task<IActionResult> DeleteModule(Guid id)
        {
            try
            {
                await _moduleService.DeleteModuleAsync(id);
                return NoContent(); // 204 No Content for successful deletion
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // For constraints like core module, parent, or being in use
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting module: {ex.Message}");
            }
        }
    }
}