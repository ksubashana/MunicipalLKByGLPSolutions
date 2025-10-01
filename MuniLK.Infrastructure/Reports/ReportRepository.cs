using Microsoft.EntityFrameworkCore;
using MuniLK.Application.Reports.Interfaces;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Infrastructure.Reports
{
    public class ReportRepository : IReportRepository
    {
        private readonly MuniLKDbContext _context;

        public ReportRepository(MuniLKDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Report report)
        {
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
        }

        public async Task<Report?> GetByIdAsync(Guid id)
        {
            return await _context.Reports.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Report>> GetAllAsync(Guid? tenantId)
        {
            return await _context.Reports
                .Where(r => r.TenantId == tenantId)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var report = await _context.Reports.FirstOrDefaultAsync(r => r.Id == id);
            if (report != null)
            {
                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }
    
    }
}
