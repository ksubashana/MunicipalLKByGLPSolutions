using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MuniLK.Application.Documents.DTOs
{
    /// <summary>
    /// DTO for uploading a document, including metadata, category (as LookupId), and the file itself.
    /// </summary>
    // MuniLK.Application.Documents.DTOs/UploadDocumentRequest.cs
    public class UploadDocumentRequest
    {
        [Required]
        public Guid DocumentTypeId { get; set; } // This should be the Lookup.Id for "Passport", "Application Form", etc.
        public string? DocumentType { get; set; } // This should be the Lookup.Id for "Passport", "Application Form", etc.

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public IFormFile File { get; set; } = null!;
        public Guid? ModuleId { get; set; } // e.g., "BuildingPlan"
        public Guid? EntityId { get; set; } // Entity primary key
        public Guid? TenantId { get; set; } // Entity primary key

        public string? LinkContext { get; set; } // Optional, e.g. "Blueprint"
        public bool IsPrimary { get; set; } = false;
    }
}