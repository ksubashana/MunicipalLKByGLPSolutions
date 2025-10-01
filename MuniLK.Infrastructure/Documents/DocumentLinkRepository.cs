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
    public class DocumentLinkRepository : IDocumentLinkRepository
    {
        private readonly MuniLKDbContext _context;

        public DocumentLinkRepository(MuniLKDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DocumentLink link, CancellationToken cancellationToken)
        {
            await _context.DocumentLinks.AddAsync(link);
            //await _context.SaveChangesAsync(); Handled from Unit of work
        }

        public async Task<List<DocumentLink>> GetLinksForEntityAsync(Guid moduleId, Guid entityId)
        {
            return await _context.DocumentLinks
                .Where(dl => dl.ModuleId == moduleId && dl.EntityId == entityId)
                .Include(dl => dl.Document)
                .ToListAsync();
        }


        public async Task<List<DocumentLink>> GetLinksForDocumentAsync(Guid documentId)
        {
            return await _context.DocumentLinks
                .Where(dl => dl.DocumentId == documentId)
                .ToListAsync();
        }

        public async Task RemoveAsync(Guid linkId)
        {
            var link = await _context.DocumentLinks.FindAsync(linkId);
            if (link != null)
            {
                _context.DocumentLinks.Remove(link);
                await _context.SaveChangesAsync();
            }
        }
    }
    }
