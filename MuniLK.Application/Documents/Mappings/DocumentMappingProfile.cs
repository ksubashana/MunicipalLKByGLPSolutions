// MuniLK.Application.Documents.DTOs/DocumentMappingProfile.cs
using MuniLK.Application.Documents.DTOs; // For UploadDocumentRequest and DocumentResponse
using MuniLK.Domain.Entities; // For the Document entity
using System;
using System.IO; // Needed for Path.GetExtension

public static class DocumentMappingProfile
{
    /// <summary>
    /// Maps an UploadDocumentRequest DTO to a Document entity.
    /// This method requires additional data generated during the upload process (blobPath, file properties).
    /// </summary>
    public static Document ToEntity(
        this UploadDocumentRequest request,
        Guid? tenantId,
        Guid documentId,
        string blobPath, // Correctly added
        long fileSize,   // Correctly added
        string contentType, // Correctly added
        string fileExtension, // Correctly added
         string? uploadedBy = null  ) // Optional: if you get this from ICurrentUserService
    {
        // No need for 'if (dto == null)' check here, as it's an extension method on 'request'.
        // If 'request' itself is null, a NullReferenceException will occur naturally,
        // which is usually desired behavior at this point in the pipeline (implies validation failed earlier).

        return new Document
        {
            Id = documentId, // Use the provided unique documentId
            DocumentTypeId = request.DocumentTypeId,
            Description = request.Description,
            FileName = request.File.FileName,       // Original filename
            FileExtension = fileExtension,          // Derived from original filename
            ContentType = contentType,              // From IFormFile
            FileSize = fileSize,                    // From IFormFile
            BlobPath = blobPath,                    // Path where the blob is stored
            TenantId = tenantId,
            IsActive = true, // Default to active on upload
            UploadedDate = DateTime.UtcNow,
            UploadedBy = uploadedBy, // Uncomment and provide if using ICurrentUserService
            // LastModifiedDate and LastModifiedBy are usually set on updates, not initial creation
        };
    }

    /// <summary>
    /// Maps a Document entity to a DocumentResponse DTO for API consumption.
    /// </summary>
    public static DocumentResponse ToResponse(this Document entity)
    {
        // Add null check for entity if there's a chance it could be null when mapping.
        // For query results, it's often better for the query handler to return null directly.
        if (entity == null)
            return null;

        return new DocumentResponse
        {
            Id = entity.Id,
            FileName = entity.FileName,
            FileExtension = entity.FileExtension,
            ContentType = entity.ContentType,
            FileSize = entity.FileSize,
            Description = entity.Description,
            DocumentTypeId = entity.DocumentTypeId,
            DocumentStatusId = entity.DocumentStatusId,
            // BlobPath = entity.BlobPath, // DO NOT EXPOSE THIS DIRECTLY!
            UploadedDate = entity.UploadedDate,
            UploadedBy = entity.UploadedBy,
            LastModifiedDate = entity.LastModifiedDate,
            LastModifiedBy = entity.LastModifiedBy
            // Add DocumentTypeName or DocumentStatusName if you eagerly load Lookups and want to expose names
            // DocumentTypeName = entity.DocumentType?.Value,
            // DocumentStatusName = entity.DocumentStatus?.Value
        };
    }
}