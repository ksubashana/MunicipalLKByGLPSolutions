using MuniLK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Documents.Interfaces
{
    public interface IDocumentLinkRepository
    {
        Task AddAsync(DocumentLink link,CancellationToken cancellationToken);
        Task<List<DocumentLink>> GetLinksForEntityAsync(Guid moduleId, Guid entityId);
        Task<List<DocumentLink>> GetLinksForDocumentAsync(Guid documentId);
        Task RemoveAsync(Guid linkId);
    }

}
