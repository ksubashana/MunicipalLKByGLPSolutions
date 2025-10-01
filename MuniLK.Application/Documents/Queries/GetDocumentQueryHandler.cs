// MuniLK.Application.Documents.Queries.GetDocument/GetDocumentQueryHandler.cs
using MediatR;
using MuniLK.Application.Documents.DTOs;
using MuniLK.Application.Documents.Interfaces;
using MuniLK.Domain.Interfaces; // For IDocumentRepository
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.Documents.Queries.GetDocument
{
    /// <summary>
    /// Handles the GetDocumentQuery, retrieving document metadata from the repository.
    /// </summary>
    public class GetDocumentQueryHandler : IRequestHandler<GetDocumentQuery, DocumentResponse?>
    {
        private readonly IDocumentRepository _repository;
        // private readonly ICurrentTenantService _tenantService; // Not strictly needed here for tenant filtering due to DB context global filter

        public GetDocumentQueryHandler(IDocumentRepository repository/*, ICurrentTenantService tenantService */)
        {
            _repository = repository;
            // _tenantService = tenantService;
        }

        public async Task<DocumentResponse?> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
        {
            // The global query filter in MuniLKDbContext (via IHasTenant and ApplyTenantFilters)
            // will automatically ensure that only documents belonging to the current tenant (or global if TenantId is null)
            // are retrieved by the repository. So, no explicit tenantId check is needed here for security.

            var document = await _repository.GetByIdAsync(request.Id);

            if (document == null)
            {
                return null; // Document not found or not accessible by the current tenant
            }

            // Map the Document entity to the DocumentResponse DTO
            return document.ToResponse();
        }
    }
}