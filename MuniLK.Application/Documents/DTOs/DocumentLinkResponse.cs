// MuniLK.Application.Documents.DTOs/DocumentLinkResponse.cs
using System;

namespace MuniLK.Application.Documents.DTOs
{
    /// <summary>
    /// Response DTO for document links with document metadata
    /// </summary>
    public class DocumentLinkResponse
    {
        public Guid LinkId { get; set; }
        public Guid DocumentId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string? Description { get; set; }
        public string? LinkContext { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime LinkedDate { get; set; }
        public string? LinkedBy { get; set; }
        public DateTime UploadedDate { get; set; }
        public string? UploadedBy { get; set; }
        public string? DocumentTypeName { get; set; }
        public string? DocumentStatusName { get; set; }
    }
}