// MuniLK.Domain/Entities/Document.cs
using System;
using System.ComponentModel.DataAnnotations;
using MuniLK.Domain.Interfaces; // Assuming IHasTenant is here

namespace MuniLK.Domain.Entities
{
    /// <summary>
    /// Represents metadata for a document stored in Blob Storage.
    /// Includes information about the document itself, its type, and storage details.
    /// </summary>
    public class Document : IHasTenant
    {
        [Key]
        public Guid Id { get; set; } // Primary key for the document metadata

        /// <summary>
        /// The unique identifier for the document in Blob Storage.
        /// This is typically the blob name or a path within the container.
        /// </summary>
        [Required]
        [MaxLength(500)] // Adjust length as needed based on your blob naming convention
        public string BlobPath { get; set; } = string.Empty;

        /// <summary>
        /// The original file name of the document when it was uploaded.
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// The file extension of the document (e.g., "pdf", "docx", "jpg").
        /// </summary>
        [Required]
        [MaxLength(10)] // e.g., ".pdf", ".docx"
        public string FileExtension { get; set; } = string.Empty;

        /// <summary>
        /// The MIME type of the document (e.g., "application/pdf", "image/jpeg").
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string ContentType { get; set; } = string.Empty;

        /// <summary>
        /// The size of the document in bytes.
        /// </summary>
        public long FileSize { get; set; } // Storing in bytes for precision

        /// <summary>
        /// Optional: A description or notes about the document.
        /// </summary>
        [MaxLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// Foreign key to the Lookup entity representing the document type (e.g., "ID Proof", "Application Form", "Survey Plan").
        /// This lookup would likely belong to a LookupCategory named "DocumentType".
        /// </summary>
        [Required]
        public Guid DocumentTypeId { get; set; }

        /// <summary>
        /// Navigation property to the associated Lookup for the document type.
        /// </summary>
        public Lookup DocumentType { get; set; } = null!;

        /// <summary>
        /// Optional: Foreign key to a Lookup entity representing the status of the document (e.g., "Pending Review", "Approved", "Rejected").
        /// This lookup would likely belong to a LookupCategory named "DocumentStatus".
        /// </summary>
        public Guid? DocumentStatusId { get; set; }

        /// <summary>
        /// Navigation property to the associated Lookup for the document status.
        /// </summary>
        public Lookup? DocumentStatus { get; set; }

        /// <summary>
        /// The TenantId this document metadata belongs to.
        /// </summary>
        public Guid? TenantId { get; set; } // Inherited from IHasTenant

        public bool IsActive { get; set; } = true;
        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;
        public string? UploadedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}