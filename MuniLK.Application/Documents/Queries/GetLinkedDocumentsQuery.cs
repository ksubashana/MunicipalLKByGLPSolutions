// MuniLK.Application.Documents.Queries/GetLinkedDocumentsQuery.cs
using MediatR;
using MuniLK.Application.Documents.DTOs;
using System;
using System.Collections.Generic;

namespace MuniLK.Application.Documents.Queries
{
    /// <summary>
    /// Query to retrieve documents linked to a specific entity (Building Plan Application)
    /// </summary>
    public class GetLinkedDocumentsQuery : IRequest<List<DocumentLinkResponse>>
    {
        public Guid ModuleId { get; set; }
        public Guid EntityId { get; set; }
        public string? LinkContext { get; set; }

        public GetLinkedDocumentsQuery(Guid moduleId, Guid entityId, string? linkContext = null)
        {
            ModuleId = moduleId;
            EntityId = entityId;
            LinkContext = linkContext;
        }
    }
}