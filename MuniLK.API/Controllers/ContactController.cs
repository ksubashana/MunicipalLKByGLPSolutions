using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.Contact.Commands.DeleteContact;
using MuniLK.Application.Contact.Commands.UpdateContact;
using MuniLK.Application.Contact.DTOs;
using MuniLK.Application.Contact.Queries.GetAllContacts;
using MuniLK.Application.Contact.Queries.GetContactById;
using MuniLK.Application.Contacts.Commands;
using MuniLK.Application.Contacts.Queries.SearchContacts;
using MuniLK.Application.Generic.Result;
using MuniLK.Application.PropertiesLK.Queries.SearchProperty;
using Syncfusion.Blazor;

namespace MuniLK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IMediator mediator, ILogger<ContactController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: api/contacts
        /// <summary>
        /// Gets all active contacts.
        /// </summary>
        /// <returns>A list of ContactResponse DTOs.</returns>
        /// 
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ContactResponse>), 200)]
        public async Task<ActionResult<IEnumerable<ContactResponse>>> GetAllContacts()
        {
            var query = new GetAllContactsQuery();
            var contacts = await _mediator.Send(query);
            return Ok(contacts);
        }

        // GET: api/contacts/{id}
        /// <summary>
        /// Gets a contact by its unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the contact.</param>
        /// <returns>A ContactResponse DTO if found, otherwise 404 Not Found.</returns>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ContactResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ContactResponse>> GetContactById(Guid id)
        {
            var query = new GetContactByIdQuery(id);
            var contact = await _mediator.Send(query);

            if (contact == null)
            {
                return NotFound($"Contact with ID {id} not found.");
            }
            return Ok(contact);
        }

        // POST: api/contacts
        /// <summary>
        /// Creates a new contact.
        /// </summary>
        /// <param name="request">The contact creation request DTO.</param>
        /// <returns>The ID of the newly created contact if successful, otherwise 400 Bad Request.</returns>
        [HttpPost("CreateContact")]
        [ProducesResponseType(typeof(Guid), 201)] // 201 Created
        [ProducesResponseType(400)] // Bad Request
        public async Task<ActionResult<Result<ContactResponse>>> CreateContact([FromBody] CreateContactRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new CreateContactCommand(request));

            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);

            //return CreatedAtAction(nameof(GetContactById), new { id = result.Data!.Id }, result);
        }

        // PUT: api/contacts/{id}
        /// <summary>
        /// Updates an existing contact.
        /// </summary>
        /// <param name="id">The unique ID of the contact to update.</param>
        /// <param name="request">The contact update request DTO.</param>
        /// <returns>204 No Content if successful, 404 Not Found if contact does not exist, 400 Bad Request.</returns>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)] // No Content
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(404)] // Not Found
        public async Task<IActionResult> UpdateContact(Guid id, [FromBody] UpdateContactRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new UpdateContactCommand(id, request);
            var result = await _mediator.Send(command);

            if (!result)
            {
                // If the handler returns false, it implies the contact was not found
                return NotFound($"Contact with ID {id} not found or update failed.");
            }
            return NoContent(); // 204 No Content typically for successful PUT updates
        }

        // DELETE: api/contacts/{id}
        /// <summary>
        /// Deletes a contact by its unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the contact to delete.</param>
        /// <returns>204 No Content if successful, 404 Not Found if contact does not exist.</returns>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)] // No Content
        [ProducesResponseType(404)] // Not Found
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            var command = new DeleteContactCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
            {
                // If the handler returns false, it implies the contact was not found
                return NotFound($"Contact with ID {id} not found.");
            }
            return NoContent(); // 204 No Content
        }


        [HttpPost("search")]
        public async Task<object> Search([FromBody] DataManagerRequest DataManagerRequest)
        {
            string filterValue = "";
            if (DataManagerRequest.Where != null && DataManagerRequest.Where.Any())
            {
                var firstFilter = DataManagerRequest.Where[0];

                var field = firstFilter.Field;      // e.g. "Name"
                var operatorType = firstFilter.Operator; // e.g. "contains"
                var value = firstFilter.value;      // <-- "aa"

                // You can cast value if needed
                filterValue = value?.ToString();
            }

            var DataSource = await _mediator.Send(new SearchContactsQuery(filterValue));

            // Handling searching operation.
            //if (DataManagerRequest.Search != null && DataManagerRequest.Search.Count > 0)
            //{
            //    DataSource = DataOperations.PerformSearching(DataSource, DataManagerRequest.Search);
            //    // Add custom logic here if needed and remove above method.
            //}

            // Get the total records count.
            //return new { result = DataSource, count = totalRecordsCount };
            return DataSource;
        }

    }

}
    
