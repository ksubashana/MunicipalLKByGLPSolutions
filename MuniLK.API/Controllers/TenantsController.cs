using MediatR;
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.Tenants.Commands;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TenantsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("CreateTenant")]
    public async Task<IActionResult> CreateTenant([FromBody] CreateTenantRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = new CreateTenantCommand
        {
            Name = request.Name,
            Subdomain = request.Subdomain,
            ContactEmail = request.ContactEmail
        };

        var tenantId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTenantById), new { id = tenantId }, new { tenantId });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTenantById(Guid id)
    {
        // Optional: Add this if you plan to return tenant info later
        return Ok(); // Stub
    }
}
