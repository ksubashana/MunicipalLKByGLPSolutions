using MediatR;
using MuniLK.Application.Documents.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Documents.Queries
{
    /// <summary>
    /// Query to retrieve the actual file content for a specific document.
    /// </summary>
    public class GetDocumentDownloadQuery : IRequest<DocumentDownloadResult?>
    {
        public Guid Id { get; set; }

        public GetDocumentDownloadQuery(Guid id)
        {
            Id = id;
        }
    }
}
