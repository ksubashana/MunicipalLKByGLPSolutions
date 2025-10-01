using MuniLK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Documents.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Document> GetByIdAsync(Guid id);
        Task AddAsync(Document document,CancellationToken cancellationToken);
        Task UpdateAsync(Document document);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Document>> GetAllAsync();
    }
}
