using MediatR;
using Microsoft.Extensions.Logging;
using MuniLK.Application.Documents.Commands.UploadDocument;
using MuniLK.Application.Documents.DTOs;
using MuniLK.Application.Documents.Interfaces;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.Services;
using MuniLK.Applications.Interfaces;
using MuniLK.Domain.Constants;
using MuniLK.Domain.Entities;
using MuniLK.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, Guid?>
{
    private readonly IDocumentRepository _repository;
    private readonly IDocumentLinkRepository _linkRepository;
    private readonly IBlobStorageService _blobStorageService;
    private readonly ICurrentTenantService _tenantService;
    private readonly ILookupService _lookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<UploadDocumentCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UploadDocumentCommandHandler(
        IDocumentRepository repository,
        IDocumentLinkRepository linkRepository,
        IBlobStorageService blobStorageService,
        ICurrentTenantService tenantService,
        ILookupService lookupService,
        ICurrentUserService currentUserService,ILogger<UploadDocumentCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _linkRepository = linkRepository;
        _blobStorageService = blobStorageService;
        _tenantService = tenantService;
        _lookupService = lookupService;
        _currentUserService = currentUserService;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid?> Handle(UploadDocumentCommand command, CancellationToken cancellationToken)
    {
        //Guid? tenantId = _tenantService.GetTenantId();
        //if (!tenantId.HasValue)
        //{
        //    throw new UnauthorizedAccessException("Tenant ID must be available to upload documents.");

        //}

        UploadDocumentRequest request = command.Request;
        Guid documentId = Guid.NewGuid();;

        //string? documentTypeValue = await _lookupService.GetLookupValueForCategoryAsync(
        //   request.DocumentTypeId,
        //   LookupCategoryNames.DocumentType.ToString());

        string safeDocumentTypeValue = SanitizePathSegment(request.DocumentType);

        if (string.IsNullOrWhiteSpace(request.DocumentType))
        {
            // Handle case where DocumentTypeId is invalid or doesn't belong to DocumentType category
            throw new ArgumentException($"Invalid DocumentTypeId '{request.DocumentTypeId}' or it does not belong to the '{LookupCategoryNames.DocumentType}' category.");
        }
        string fileExtension = Path.GetExtension(request.File.FileName)?.ToLowerInvariant() ?? "";
        string generatedBlobPath = $"{request.TenantId}/Documents/{safeDocumentTypeValue}/{documentId}{fileExtension}";

        string? uploadedBlobPath = null; // Declare outside try block for scope

        try
        {
            // 1. Upload file to blob storage
            // If this fails, the catch block won't execute for DB, which is fine.
            uploadedBlobPath = await _blobStorageService.UploadAsync(
                generatedBlobPath,
                request.File.OpenReadStream(),
                request.File.FileName,
                request.File.ContentType,
                cancellationToken);

            string? uploadedBy = _currentUserService.UserName ?? _currentUserService.UserId;

            // 2. Prepare document entity
            Document document = request.ToEntity(
                request.TenantId,
                documentId,
                uploadedBlobPath, // Use the path returned from blob storage
                request.File.Length,
                request.File.ContentType,
                fileExtension,
                uploadedBy
            );

            // 3. Save document metadata to the database
            await _repository.AddAsync(document,cancellationToken);

            if (request.ModuleId.HasValue && request.EntityId.HasValue)
            {
                var documentLink = new DocumentLink
                {
                    Id = Guid.NewGuid(),
                    DocumentId = document.Id,
                    ModuleId = request.ModuleId.Value,
                    EntityId = request.EntityId.Value,
                    LinkContext = request.LinkContext,
                    IsPrimary = request.IsPrimary,
                    TenantId = request.TenantId,
                    LinkedBy = _currentUserService.UserName ?? _currentUserService.UserId,
                    LinkedDate = DateTime.UtcNow
                };

                await _linkRepository.AddAsync(documentLink,cancellationToken);

                
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return document.Id;
        }
        catch (Exception ex)
        {
            // Log the exception first
            _logger.LogError(ex, "Failed to upload document or save metadata for document ID {DocumentId}.", documentId); // Assuming you have a logger injected

            // COMPENSATION LOGIC: If DB save failed AND blob upload succeeded, delete the blob.
            if (!string.IsNullOrEmpty(uploadedBlobPath))
            {
                try
                {
                    // Attempt to delete the partially uploaded blob to prevent orphaned files
                    await _blobStorageService.DeleteAsync(uploadedBlobPath, cancellationToken);
                    _logger.LogWarning("Compensated: Deleted orphaned blob at path {BlobPath} due to database transaction failure for document ID {DocumentId}.", uploadedBlobPath, documentId);
                }
                catch (Exception deleteEx)
                {
                    // This is critical: if compensation fails, you need an alert!
                    // This blob will remain orphaned. Consider a background cleanup job.
                    _logger.LogError(deleteEx, "CRITICAL: Failed to delete orphaned blob at path {BlobPath} after database transaction failure for document ID {DocumentId}.", uploadedBlobPath, documentId);
                    // You might want to log this to a separate system, or send an alert.
                }
            }

            // Re-throw the original exception to propagate the failure to the caller
            throw;
        }
    }
    /// <summary>
    /// Helper method to sanitize a string for use as a path segment in blob storage.
    /// </summary>
    private string SanitizePathSegment(string input)
    {
        // Replace spaces with hyphens
        string sanitized = input.Replace(" ", "-");
        // Remove any characters that are not alphanumeric, hyphens, or underscores
        sanitized = System.Text.RegularExpressions.Regex.Replace(sanitized, "[^a-zA-Z0-9_-]", "");
        // Convert to lowercase for consistency
        sanitized = sanitized.ToLowerInvariant();
        return sanitized;
    }
}