using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuniLK.Application.LicensesLK.DTOs;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;


namespace MuniLK.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class LicenseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LicenseController> _logger;

        public LicenseController(IMediator mediator , ILogger<LicenseController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLicenseRequest license)
        {
            var id = await _mediator.Send(new CreateLicenseCommand(license));
            return CreatedAtAction(nameof(GetById), new { id }, license);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<License>> GetById(Guid id)
        {
            var license = await _mediator.Send(new GetLicenseByIdQuery(id));
            if (license == null) return NotFound();
            return license;
        }
        [HttpGet("GetAllLicenses")]
        public async Task<ActionResult<IEnumerable<License>>> GetLicenses()
        {
            _logger.LogInformation("Fetching all license with ID - sample log");

            var licenses = await _mediator.Send(new GetAllLicensesQuery());
            return Ok(licenses); // Fix: Wrap the result in Ok() to return ActionResult<IEnumerable<License>>
        }
    }
}