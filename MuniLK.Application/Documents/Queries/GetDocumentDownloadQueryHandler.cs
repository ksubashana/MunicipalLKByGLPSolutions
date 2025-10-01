// MuniLK.Application.Documents.Queries.GetDocumentDownload/GetDocumentDownloadQueryHandler.cs
using MediatR;
using MuniLK.Application.Documents.DTOs;
using MuniLK.Application.Documents.Interfaces; // For IBlobStorageService
using MuniLK.Domain.Interfaces; // For IDocumentRepository
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.Documents.Queries.GetDocumentDownload
{
    /// <summary>
    /// Handles the GetDocumentDownloadQuery, retrieving the actual file content from blob storage.
    /// </summary>
    public class GetDocumentDownloadQueryHandler : IRequestHandler<GetDocumentDownloadQuery, DocumentDownloadResult?>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IBlobStorageService _blobStorageService;
        // private readonly ICurrentTenantService _tenantService; // Global query filter handles this

        public GetDocumentDownloadQueryHandler(
            IDocumentRepository documentRepository,
            IBlobStorageService blobStorageService
            /* ICurrentTenantService tenantService */)
        {
            _documentRepository = documentRepository;
            _blobStorageService = blobStorageService;
            // _tenantService = tenantService;
        }

        public async Task<DocumentDownloadResult?> Handle(GetDocumentDownloadQuery request, CancellationToken cancellationToken)
        {
            // 1. Get the document metadata from the database
            // The global query filter will ensure only documents accessible to the current tenant are retrieved.
            var document = await _documentRepository.GetByIdAsync(request.Id);

            if (document == null)
            {
                // Document not found or not accessible by the current tenant.
                return null;
            }

            // 2. Use the BlobPath from the document metadata to download from blob storage
            var downloadResultTuple = await _blobStorageService.DownloadAsync(document.BlobPath, cancellationToken);
            if (downloadResultTuple == null) // Check if the nullable tuple itself is null
            {
                // Blob not found even if metadata exists (e.g., deleted from storage but not DB)
                // You might log this as an inconsistency
                return null;
            }

            // Now, deconstruct the non-nullable Value from the tuple
            var (content, contentType, fileName) = downloadResultTuple.Value; // Access .Value before deconstructing
            if (content == null)
            {
                // Blob not found even if metadata exists (e.g., deleted from storage but not DB)
                // You might log this as an inconsistency
                return null;
            }

            return new DocumentDownloadResult
            {
                Content = content,
                ContentType = contentType,
                // Use the FileName from the DB metadata, or the one returned by blob storage if available.
                FileName = document.FileName // Prefer filename from DB for consistency
            };
        }
    }
}