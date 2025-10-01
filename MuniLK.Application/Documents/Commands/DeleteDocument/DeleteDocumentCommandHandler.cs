// MuniLK.Application.Documents.Commands.DeleteDocument/DeleteDocumentCommandHandler.cs
using MediatR;
using MuniLK.Application.Documents.Interfaces; // For IBlobStorageService
using MuniLK.Domain.Interfaces; // For IDocumentRepository
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.Documents.Commands.DeleteDocument
{
    /// <summary>
    /// Handles the DeleteDocumentCommand, removing both document metadata and the actual blob.
    /// </summary>
    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, Unit>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IBlobStorageService _blobStorageService;
        // private readonly ICurrentTenantService _tenantService; // Global query filter handles tenant access

        public DeleteDocumentCommandHandler(
            IDocumentRepository documentRepository,
            IBlobStorageService blobStorageService
            /* ICurrentTenantService tenantService */)
        {
            _documentRepository = documentRepository;
            _blobStorageService = blobStorageService;
            // _tenantService = tenantService;
        }

        public async Task<Unit> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            // 1. Retrieve the document metadata from the database.
            // The global query filter ensures only documents belonging to the current tenant can be found.
          MuniLK.Domain.Entities.Document document = await _documentRepository.GetByIdAsync(request.Id);

            if (document == null)
            {
                // If the document doesn't exist (or is not accessible by the current tenant),
                // we consider the operation implicitly successful (idempotent delete).
                // Or you could throw a NotFoundException if you prefer an explicit error for non-existence.
                return Unit.Value;
            }

            // 2. Delete the actual blob from storage.
            // It's generally safer to delete the blob first. If blob deletion fails, you still have the DB record.
            // If DB deletion fails, you risk an orphaned blob.
            // The DeleteAsync method on IBlobStorageService handles existence checks internally.
            await _blobStorageService.DeleteAsync(document.BlobPath, cancellationToken);

            // 3. Delete the document metadata from the database.
            await _documentRepository.DeleteAsync(request.Id);

            // MediatR's Unit.Value indicates a successful void operation.
            return Unit.Value;
        }
    }
}