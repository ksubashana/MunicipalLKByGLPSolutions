using BoldReports.RDL.DOM;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.PropertiesLK;
using MuniLK.Application.PropertiesLK.DTOs;
using MuniLK.Application.PropertiesLK.Queries.SearchProperty;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace MuniLK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PropertiesController> _logger;
        private readonly IPropertyRepository _propertyRepository;

        public PropertiesController(IMediator mediator, ILogger<PropertiesController> logger, IPropertyRepository propertyRepository)
        {
            _mediator = mediator;
            _logger = logger;
            _propertyRepository = propertyRepository;
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreatePropertyRequest request)
        {
            var command = new CreatePropertyCommand(request);
            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id = id }, new { propertyId = id });
        }

        // NEW: Strongly-typed GET by Guid for detailed property info
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var entity = await _propertyRepository.GetByIdAsync(id, ct);
            if (entity is null) return NotFound();

            var dto = new PropertyResponse
            {
                Id = entity.Id,
                PropertyId = entity.PropertyId,
                Address = entity.Address,
                TitleDeedNumber = entity.TitleDeedNumber,
                AssessmentValue = entity.AssessmentValue,
                IsCommercialUse = entity.IsCommercialUse
            };

            return Ok(dto);
        }

        // Legacy placeholder (kept to avoid breaking clients expecting int id)
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(new { id });
        }

        [HttpPost("search")]
        public async Task<object> Search([FromBody] DataManagerRequest DataManagerRequest)
        {
            string filterValue = "";
            if (DataManagerRequest.Where != null && DataManagerRequest.Where.Any())
            {
                var firstFilter = DataManagerRequest.Where[0];
                var value = firstFilter.value;
                filterValue = value?.ToString();
            }

            var DataSource = await _mediator.Send(new SearchPropertiesQuerySync(filterValue));
            return DataSource;
        }
    }
}

public class AutoCompleteResponse<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int Count { get; set; }
}