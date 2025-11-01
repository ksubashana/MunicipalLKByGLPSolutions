// MuniLK.API/Controllers/LookupController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.Services;
using MuniLK.Application.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MuniLK.Application.BuildingAndPlanning.DTOs;

namespace MuniLK.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class LookupsController : ControllerBase
    {
        private readonly ILookupService _lookupService;
        private readonly ILogger<LookupsController> _logger;

        public LookupsController(ILookupService lookupService, ILogger<LookupsController> logger)
        {
            _lookupService = lookupService;
            _logger = logger;
        }

        // --- Lookup Categories Endpoints ---

        /// <summary>
        /// Retrieves all active lookup categories, combining global and tenant-specific ones.
        /// </summary>
        /// <returns>A list of LookupCategoryDto objects.</returns>
        [HttpGet("categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<LookupCategoryDto>>> GetLookupCategories()
        {
            try
            {
                _logger.LogInformation("Fetching all lookup categories.");
                var categories = await _lookupService.GetLookupCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching lookup categories.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving lookup categories.");
            }
        }

        [HttpGet("categories/id/byname/{name}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLookupCategoryIdByName(string name)
        {
            try
            {
                _logger.LogInformation("Getting lookup category ID for name: {Name}", name);

                var categoryId = await _lookupService.GetLookupCategoryIdByNameAsync(name);
                if (categoryId == null)
                {
                    return NotFound($"Lookup category with name '{name}' not found.");
                }

                return Ok(categoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category ID for name: {Name}", name);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving category ID.");
            }
        }

        [HttpGet("id/bycategoryandvalue/{categoryId}/{value}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLookupIdByCategoryIdAndValue(Guid categoryId, string value)
        {
            try
            {
                _logger.LogInformation("Getting lookup ID for CategoryId: {CategoryId} and Value: {Value}", categoryId, value);

                var lookupId = await _lookupService.GetLookupIdByCategoryIdAndValueAsync(categoryId, value);
                if (lookupId == null)
                {
                    return NotFound($"Lookup with category ID '{categoryId}' and value '{value}' not found.");
                }

                return Ok(lookupId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lookup ID for CategoryId: {CategoryId} and Value: {Value}", categoryId, value);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the lookup ID.");
            }
        }
        /// <summary>
        /// Adds a new lookup category. Can be global or tenant-specific.
        /// </summary>
        /// <param name="request">The request DTO for the new category.</param>
        /// <returns>The ID of the newly created lookup category.</returns>
        [HttpPost("CreateCategories")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> AddLookupCategory([FromBody] AddLookupCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Attempting to add new lookup category: Name='{Name}', IsGlobal='{IsGlobal}'",
                    request.Name, request.IsGlobal);

                var newId = await _lookupService.AddLookupCategoryAsync(request);

                _logger.LogInformation("Successfully added lookup category with ID: {LookupCategoryId}", newId);
                return StatusCode(StatusCodes.Status201Created, newId);
            }
            catch (InvalidOperationException ioEx)
            {
                _logger.LogWarning(ioEx, "Failed to add lookup category due to business rule: {Message}", ioEx.Message);
                return BadRequest(ioEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding lookup category: Name='{Name}'", request.Name);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the lookup category.");
            }
        }

        // --- Lookup Values Endpoints ---

        /// <summary>
        /// Retrieves a list of lookup values for a given category by its programmatic name.
        /// Combines global and tenant-specific values for the current tenant.
        /// </summary>
        /// <param name="categoryName">The programmatic name of the lookup category (e.g., "PropertyType").</param>
        /// <returns>A list of unique LookupValueDto objects.</returns>
        [HttpGet("values/byname/{categoryName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // If category name not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<LookupDto>>> GetLookupValuesByCategoryName(string categoryName)
        {
            try
            {
                _logger.LogInformation("Fetching lookup values for category name: {CategoryName}", categoryName);
                var values = await _lookupService.GetLookupValuesByCategoryNameAsync(categoryName);
                if (values == null || !values.Any())
                {
                    // This might return 200 OK with empty list, or 404 if category not found.
                    // Current service returns empty list if category not found, so 200 OK is fine.
                    // If you want 404, the service would need to return null or throw specific exception.
                    _logger.LogInformation("No lookup values found for category name: {CategoryName}", categoryName);
                }
                return Ok(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching lookup values for category name: {CategoryName}", categoryName);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving lookup values.");
            }
        }

        /// <summary>
        /// Retrieves a list of lookup values for a given category by its GUID ID.
        /// Combines global and tenant-specific values for the current tenant.
        /// </summary>
        /// <param name="categoryId">The GUID ID of the lookup category.</param>
        /// <returns>A list of unique LookupValueDto objects.</returns>
        [HttpGet("values/byid/{categoryId:guid}")] // :guid constraint ensures it's a GUID
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<LookupDto>>> GetLookupValuesByCategoryId(Guid categoryId)
        {
            try
            {
                _logger.LogInformation("Fetching lookup values for category ID: {CategoryId}", categoryId);
                var values = await _lookupService.GetLookupValuesByCategoryIdAsync(categoryId);
                return Ok(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching lookup values for category ID: {CategoryId}", categoryId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving lookup values.");
            }
        }

        /// <summary>
        /// Adds a new lookup value. Can be global or tenant-specific based on the IsGlobal flag.
        /// </summary>
        /// <param name="request">The request DTO containing the category ID, value, and global flag.</param>
        /// <returns>The ID of the newly created lookup value.</returns>
        [HttpPost("AddLookup")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> AddLookupValue([FromBody] AddLookupRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Attempting to add new lookup value: CategoryId='{CategoryId}', Value='{Value}', IsGlobal='{IsGlobal}'",
                    request.LookupCategoryId, request.Value, request.IsGlobal);

                var newId = await _lookupService.AddLookupValueAsync(request);

                _logger.LogInformation("Successfully added lookup value with ID: {LookupValueId}", newId);
                return StatusCode(StatusCodes.Status201Created, newId);
            }
            catch (InvalidOperationException ioEx)
            {
                _logger.LogWarning(ioEx, "Failed to add lookup value due to business rule: {Message}", ioEx.Message);
                return BadRequest(ioEx.Message); // e.g., "Lookup value already exists"
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding lookup value: CategoryId='{CategoryId}', Value='{Value}'", request.LookupCategoryId, request.Value);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the lookup value.");
            }
        }

        /// <summary>
        /// Adds a new lookup value. Can be global or tenant-specific based on the IsGlobal flag.
        /// </summary>
        /// <param name="request">The request DTO containing the category ID, value, and global flag.</param>
        /// <returns>The ID of the newly created lookup value.</returns>
        // Modified endpoint to accept multiple lookup requests
        [HttpPost("AddMultipleLookup")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // If authentication/authorization is applied
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Guid>>> AddLookupValuesBatch([FromBody] IEnumerable<AddLookupRequest> requests)
        {
            if (requests == null || !requests.Any()) // Check if the incoming collection is empty
            {
                return BadRequest("No lookup values provided for batch creation.");
            }

            // You might want to run ModelState.IsValid for each item if you prefer
            // granular error messages for each entry. For simplicity, we'll
            // just check if *any* model state is invalid if you have per-item validation.
            // If you have a custom validation pipeline, it might handle this automatically.
            foreach (var request in requests)
            {
                // This rudimentary check can be expanded for more detailed validation messages per item
                if (string.IsNullOrWhiteSpace(request.Value) || request.LookupCategoryId == Guid.Empty)
                {
                    ModelState.AddModelError("", "Each lookup item must have a valid LookupCategoryId and Value.");
                    return BadRequest(ModelState);
                }
            }

            if (!ModelState.IsValid) // This checks overall model state, which might not catch per-item issues without specific setup
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Attempting to add {Count} new lookup values in batch.", requests.Count());

                // Assume your service now has a method that accepts a collection
                var newIds = await _lookupService.AddLookupValuesBatchAsync(requests);

                _logger.LogInformation("Successfully added {Count} lookup values. IDs: {LookupValueIds}", newIds.Count(), string.Join(", ", newIds));
                return StatusCode(StatusCodes.Status201Created, newIds);
            }
            catch (InvalidOperationException ioEx)
            {
                _logger.LogWarning(ioEx, "Failed to add lookup values batch due to business rule: {Message}", ioEx.Message);
                return BadRequest(ioEx.Message); // e.g., "Lookup value already exists"
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding lookup values batch.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the lookup values.");
            }
        }

        // NEW: Entity Option Selection Endpoints (moved from InspectionController)
        [HttpGet("entity-options/selections")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Guid>>> GetEntityOptionSelections([FromQuery] Guid entityId, [FromQuery] string entityType, [FromQuery] Guid moduleId)
        {
            if (entityId == Guid.Empty || string.IsNullOrWhiteSpace(entityType) || moduleId == Guid.Empty)
                return BadRequest("entityId, entityType and moduleId are required.");

            var ids = await _lookupService.GetEntityOptionSelectionsAsync(entityId, entityType, moduleId);
            return Ok(ids);
        }

        [HttpDelete("entity-options/selections")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteEntityOptionSelections([FromQuery] Guid entityId, [FromQuery] string entityType, [FromQuery] Guid moduleId)
        {
            if (entityId == Guid.Empty || string.IsNullOrWhiteSpace(entityType) || moduleId == Guid.Empty)
                return BadRequest("entityId, entityType and moduleId are required.");

            await _lookupService.DeleteEntityOptionSelectionsAsync(entityId, entityType, moduleId);
            return NoContent();
        }

        [HttpPost("entity-options/selections/save")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EntityOptionSelectionsResponse>> SaveEntityOptionSelections([FromBody] SaveEntityOptionSelectionsRequest request)
        {
            if (request == null || request.EntityId == Guid.Empty || string.IsNullOrWhiteSpace(request.EntityType) || request.ModuleId == Guid.Empty)
                return BadRequest("EntityId, EntityType and ModuleId are required.");

            var response = await _lookupService.SaveEntityOptionSelectionsAsync(request.EntityId, request.EntityType, request.ModuleId, request.OptionItemIds ?? new List<Guid>());
            return Ok(response);
        }

        // Hierarchy root retrieval
        [HttpGet("values/root/byname/{categoryName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<LookupDto>>> GetRootLookupsByCategory(string categoryName)
        {
            try
            {
                var roots = await _lookupService.GetRootLookupsByCategoryNameAsync(categoryName);
                return Ok(roots);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching root lookups for category {CategoryName}", categoryName);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error fetching root lookups.");
            }
        }

        // Children retrieval
        [HttpGet("values/children/{parentLookupId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<LookupDto>>> GetChildLookups(Guid parentLookupId)
        {
            try
            {
                var children = await _lookupService.GetChildLookupsAsync(parentLookupId);
                return Ok(children);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching child lookups for parent {ParentLookupId}", parentLookupId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error fetching child lookups.");
            }
        }
    }
}
