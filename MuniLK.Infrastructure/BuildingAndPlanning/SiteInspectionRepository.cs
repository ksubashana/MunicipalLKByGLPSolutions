using Microsoft.EntityFrameworkCore;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;

namespace MuniLK.Infrastructure.BuildingAndPlanning
{
    public class SiteInspectionRepository : ISiteInspectionRepository
    {
        private readonly MuniLKDbContext _context;

        public SiteInspectionRepository(MuniLKDbContext context)
        {
            _context = context;
        }

        public async Task<SiteInspection> AddAsync(SiteInspection siteInspection, CancellationToken cancellationToken = default)
        {
            var result = await _context.SiteInspections.AddAsync(siteInspection, cancellationToken);
            return result.Entity;
        }

        public async Task<SiteInspection?> GetByInspectionIdAsync(Guid inspectionId, CancellationToken cancellationToken = default)
        {
            return await _context.SiteInspections
                .FirstOrDefaultAsync(s => s.InspectionId == inspectionId, cancellationToken);
        }

        public async Task<IEnumerable<SiteInspection>> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default)
        {
            return await _context.SiteInspections
                .Where(s => s.ApplicationId == applicationId)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changes = await _context.SaveChangesAsync(cancellationToken);
            return changes > 0;
        }
    }
}