// MuniLK.Application.Documents.Queries/GetLinkedDocumentsQueryHandler.cs
using MediatR;
using MuniLK.Application.Documents.DTOs;
using MuniLK.Application.Documents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.Documents.Queries
{
    /// <summary>
    /// Handler for GetLinkedDocumentsQuery
    /// </summary>
    public class GetLinkedDocumentsQueryHandler : IRequestHandler<GetLinkedDocumentsQuery, List<DocumentLinkResponse>>
    {
        private readonly IDocumentLinkRepository _documentLinkRepository;

        public GetLinkedDocumentsQueryHandler(IDocumentLinkRepository documentLinkRepository)
        {
            _documentLinkRepository = documentLinkRepository;
        }

        public async Task<List<DocumentLinkResponse>> Handle(GetLinkedDocumentsQuery request, CancellationToken cancellationToken)
        {
            var documentLinks = await _documentLinkRepository.GetLinksForEntityAsync(request.ModuleId, request.EntityId);

            // Optional filter by LinkContext
            if (!string.IsNullOrWhiteSpace(request.LinkContext))
            {
                documentLinks = documentLinks.Where(dl => dl.LinkContext == request.LinkContext).ToList();
            }

            return documentLinks.Select(dl => new DocumentLinkResponse
            {
                LinkId = dl.Id,
                DocumentId = dl.DocumentId,
                FileName = dl.Document.FileName,
                FileExtension = dl.Document.FileExtension,
                ContentType = dl.Document.ContentType,
                FileSize = dl.Document.FileSize,
                Description = dl.Document.Description,
                LinkContext = dl.LinkContext,
                IsPrimary = dl.IsPrimary,
                LinkedDate = dl.LinkedDate,
                LinkedBy = dl.LinkedBy,
                UploadedDate = dl.Document.UploadedDate,
                UploadedBy = dl.Document.UploadedBy,
                DocumentTypeName = dl.Document.DocumentType?.Value,
                DocumentStatusName = dl.Document.DocumentStatus?.Value
            }).OrderByDescending(dl => dl.LinkedDate).ToList();
        }
    }
}