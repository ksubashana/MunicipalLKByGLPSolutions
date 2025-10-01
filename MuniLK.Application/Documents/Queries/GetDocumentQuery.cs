// MuniLK.Application.Documents.Queries.GetDocument/GetDocumentQuery.cs
using MediatR;
using MuniLK.Application.Documents.DTOs;
using System;

namespace MuniLK.Application.Documents.Queries.GetDocument
{
    /// <summary>
    /// Query to retrieve a specific document's metadata by its ID.
    /// </summary>
    public class GetDocumentQuery : IRequest<DocumentResponse?>
    {
        public Guid Id { get; set; }

        public GetDocumentQuery(Guid id)
        {
            Id = id;
        }
    }
}