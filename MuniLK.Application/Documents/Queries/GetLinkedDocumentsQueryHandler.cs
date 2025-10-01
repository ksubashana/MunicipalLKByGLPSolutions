// MuniLK.Application.Documents.Queries/GetLinkedDocumentsQueryHandler.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using MuniLK.Application.Documents.DTOs;
using MuniLK.Infrastructure.Data;
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
        private readonly MuniLKDbContext _context;

        public GetLinkedDocumentsQueryHandler(MuniLKDbContext context)
        {
            _context = context;
        }

        public async Task<List<DocumentLinkResponse>> Handle(GetLinkedDocumentsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.DocumentLinks
                .Include(dl => dl.Document)
                .ThenInclude(d => d.DocumentType)
                .Include(dl => dl.Document.DocumentStatus)
                .Where(dl => dl.ModuleId == request.ModuleId && dl.EntityId == request.EntityId);

            // Optional filter by LinkContext
            if (!string.IsNullOrWhiteSpace(request.LinkContext))
            {
                query = query.Where(dl => dl.LinkContext == request.LinkContext);
            }

            var documentLinks = await query
                .OrderByDescending(dl => dl.LinkedDate)
                .ToListAsync(cancellationToken);

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
                DocumentTypeName = dl.Document.DocumentType?.DisplayName ?? dl.Document.DocumentType?.Name,
                DocumentStatusName = dl.Document.DocumentStatus?.DisplayName ?? dl.Document.DocumentStatus?.Name
            }).ToList();
        }
    }
}