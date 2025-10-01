using Microsoft.EntityFrameworkCore;
using MuniLK.Application.Documents.Interfaces;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Infrastructure.Documents
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly MuniLKDbContext _context;

        public DocumentRepository(MuniLKDbContext context)
        {
            _context = context;
        }

        public async Task<Document> GetByIdAsync(Guid id)
        {
            return await _context.Documents.FindAsync(id);
        }

        public async Task AddAsync(Document document, CancellationToken cancellationToken)
        {
            //_context.Documents.Add(document);
            await _context.Set<Document>().AddAsync(document, cancellationToken);

            //await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Document document)
        {
            _context.Documents.Update(document);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Document>> GetAllAsync()
        {
            return await _context.Documents.ToListAsync();
        }
    }
}
