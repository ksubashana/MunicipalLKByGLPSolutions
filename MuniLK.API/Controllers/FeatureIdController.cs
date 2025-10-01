using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.FeatureId.Commands;
using MuniLK.Application.FeatureId.DTOs;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class FeatureIdController : ControllerBase
{
    private readonly IMediator _mediator;

    public FeatureIdController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] GenerateFeatureIdRequest request)
    {
        var result = await _mediator.Send(new GenerateFeatureIdCommand
        {
            ConfigKey = request.ConfigKey
        });

        return Ok(result);
    }
}
