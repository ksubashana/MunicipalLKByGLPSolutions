using MediatR;
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.Tenants.Commands;
using MuniLK.Application.Tenants;
using MuniLK.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ITenantRepository _tenantRepository;

    public TenantsController(IMediator mediator, ITenantRepository tenantRepository)
    {
        _mediator = mediator;
        _tenantRepository = tenantRepository;
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

    [HttpGet("list")]
    public async Task<IActionResult> GetAllTenants(CancellationToken cancellationToken)
    {
        var tenants = await _tenantRepository.GetAllAsync(cancellationToken);
        var result = tenants.Select(t => new { Id = t.TenantId, t.Name, t.Subdomain });
        return Ok(result);
    }
}
