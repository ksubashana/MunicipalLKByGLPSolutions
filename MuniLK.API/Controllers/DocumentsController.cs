
    using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.Documents.Commands.DeleteDocument;
using MuniLK.Application.Documents.Commands.UploadDocument;
    using MuniLK.Application.Documents.DTOs;
using MuniLK.Application.Documents.Queries;
using MuniLK.Application.Documents.Queries.GetDocument;
    using System;
    using System.Threading.Tasks;

namespace MuniLK.API.Controllers
{
    [ApiController]
    [Route("api/documents")] // TenantId is part of the route
    public class DocumentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Uploads a new document for a specific tenant.
        /// The file should be sent as multipart/form-data.
        /// </summary>
        [HttpPost("Upload")]
        [ProducesResponseType(typeof(List<Guid?>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadDocument()
        {

            // 1. Validate required headers
            if (!Request.Headers.TryGetValue("EntityId", out var entityIdStr) ||
                !Guid.TryParse(entityIdStr.FirstOrDefault(), out var entityId))
            {
                return BadRequest("Missing or invalid 'EntityId' (must be a GUID).");
            }

            if (!Request.Headers.TryGetValue("DocumentTypeId", out var documentTypeIdStr) ||
                !Guid.TryParse(documentTypeIdStr.FirstOrDefault(), out var documentTypeId))
            {
                return BadRequest("Missing or invalid 'DocumentTypeId' (must be a GUID).");
            }
            if (!Request.Headers.TryGetValue("DocumentType", out var DocumentType) || string.IsNullOrWhiteSpace(DocumentType))
            {
                return BadRequest("Missing 'DocumentType'.");
            }

            if (!Request.Headers.TryGetValue("ModuleId", out var moduleIdStr) ||
                !Guid.TryParse(moduleIdStr.FirstOrDefault(), out var moduleId))
            {
                return BadRequest("Missing or invalid 'ModuleId' (must be a GUID).");
            }

            if (!Request.Headers.TryGetValue("LinkContext", out var linkContext) || string.IsNullOrWhiteSpace(linkContext))
            {
                return BadRequest("Missing 'LinkContext'.");
            }

            if (!Request.Headers.TryGetValue("TenantIdClaim", out var tenantIdClaim) || string.IsNullOrWhiteSpace(tenantIdClaim))
            {
                return BadRequest("Missing 'tenantIdClaim'.");
            }
            // 2. Get uploaded files
            var files = Request.Form.Files;
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files were uploaded.");
            }

            // 3. Process each file
            var uploadedIds = new List<Guid?>();
            foreach (var file in files)
            {
                var request = new UploadDocumentRequest
                {
                    File = file,
                    DocumentTypeId = documentTypeId,
                    DocumentType = DocumentType,
                    EntityId = entityId,
                    ModuleId = moduleId,
                    LinkContext = linkContext,
                    TenantId = Guid.Parse( tenantIdClaim),
                    Description = null // Optional: can extend with another header/form field
                };

                var command = new UploadDocumentCommand(request);
                var documentId = await _mediator.Send(command);
                uploadedIds.Add(documentId);
            }

            // 4. Return result
            return Created(string.Empty, uploadedIds);
        }

        /// <summary>
        /// Retrieves metadata for a specific document by its ID.
        /// </summary>
        /// <param name="id">The unique ID of the document.</param>
        /// <returns>Document metadata if found, otherwise NotFound.</returns>
        [HttpGet("{id}")] // Defines a GET endpoint like GET /api/documents/{id}
        [ProducesResponseType(typeof(DocumentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDocument(Guid id)
        {
            var query = new GetDocumentQuery(id);
            var document = await _mediator.Send(query);

            if (document == null)
            {
                return NotFound(); // Return 404 if document doesn't exist or isn't accessible to the tenant
            }

            return Ok(document); // Return 200 OK with document metadata
        }

        /// <summary>
        /// Downloads the actual file content of a specific document by its ID.
        /// </summary>
        /// <param name="id">The unique ID of the document to download.</param>
        /// <returns>The file stream if found, otherwise NotFound.</returns>
        [HttpGet("{id}/download")] // New endpoint for download
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadDocument(Guid id)
        {
            var query = new GetDocumentDownloadQuery(id);
            var result = await _mediator.Send(query);

            if (result == null || result.Content == Stream.Null) // Check if content is actually available
            {
                return NotFound(); // Document metadata or actual blob not found/accessible
            }

            // Return the file stream using ASP.NET Core's File method
            return File(result.Content, result.ContentType, result.FileName);
        }

        /// <summary>
        /// Retrieves documents linked to a specific entity (Building Plan Application).
        /// </summary>
        /// <param name="moduleId">The Module ID (e.g., Building and Planning)</param>
        /// <param name="entityId">The Entity ID (e.g., Application ID)</param>
        /// <param name="linkContext">Optional: Filter by link context</param>
        /// <returns>List of linked documents</returns>
        [HttpGet("linked")]
        [ProducesResponseType(typeof(List<DocumentLinkResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLinkedDocuments(
            [FromQuery] Guid moduleId, 
            [FromQuery] Guid entityId, 
            [FromQuery] string? linkContext = null)
        {
            var query = new GetLinkedDocumentsQuery(moduleId, entityId, linkContext);
            var documents = await _mediator.Send(query);
            return Ok(documents);
        }

        /// <summary>
        /// Gets a preview URL for a document (same as download for now).
        /// </summary>
        /// <param name="id">The unique ID of the document to preview.</param>
        /// <returns>The file stream for preview if found, otherwise NotFound.</returns>
        [HttpGet("{id}/preview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PreviewDocument(Guid id)
        {
            // For now, preview is the same as download
            // In the future, you might want to return thumbnails or embedded viewer URLs
            return await DownloadDocument(id);
        }

        /// <summary>
        /// Deletes a document and its associated file from storage by ID.
        /// </summary>
        /// <param name="id">The ID of the document to delete.</param>
        /// <returns>NoContent if successful, NotFound if the document does not exist.</returns>
        [HttpDelete("{id}")] // Defines a DELETE endpoint like DELETE /api/documents/{id}
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Standard for successful DELETE with no content back
        [ProducesResponseType(StatusCodes.Status404NotFound)] // If you want to explicitly signal non-existence
        public async Task<IActionResult> DeleteDocument(Guid id)
        {
            var command = new DeleteDocumentCommand(id);
            // We can check if the document exists before sending the command if we want to return 404
            // more explicitly before the command executes its full logic.
            // Otherwise, the handler returns Unit.Value even if not found, implying idempotency.

            // To return 404 explicitly if not found:
            var existingDocument = await _mediator.Send(new GetDocumentQuery(id)); // Use the GetDocumentQuery
            if (existingDocument == null)
            {
                return NotFound(); // Return 404 if document doesn't exist or isn't accessible to the tenant
            }

            await _mediator.Send(command); // Execute the delete command

            return NoContent(); // Return 204 No Content for successful deletion
        }
    }
}