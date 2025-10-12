using Microsoft.EntityFrameworkCore;
using MuniLK.Application.Assignments;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Infrastructure.Assignments
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly MuniLKDbContext _context;

        public AssignmentRepository(MuniLKDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Assignment assignment)
        {
            if (assignment == null)
                throw new ArgumentNullException(nameof(assignment));

            // Find any existing assignments with same EntityId, ModuleId, and TenantId
            var existingAssignments = await _context.Assignments
                .Where(a => a.EntityId == assignment.EntityId
                            && a.ModuleId == assignment.ModuleId
                            && a.TenantId == assignment.TenantId)
                .ToListAsync();

            if (existingAssignments.Any())
            {
                _context.Assignments.RemoveRange(existingAssignments);
            }

            await _context.Assignments.AddAsync(assignment);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Assignments.FindAsync(id);
            if (entity != null)
            {
                _context.Assignments.Remove(entity);
            }
        }

        public async Task<Assignment> GetByIdAsync(Guid id)
        {
            return await _context.Assignments
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Assignment>> GetByInspectorAsync(Guid inspectorUserId, Guid tenantId)
        {
            return await _context.Assignments
                .AsNoTracking()
                .Where(a => a.AssignedTo == inspectorUserId && a.TenantId == tenantId)
                .OrderByDescending(a => a.AssignmentDate)
                .ToListAsync();
        }
        public async Task<IEnumerable<Assignment>> GetByModuleAndEntityAsync(Guid? moduleId, Guid? entityId, Guid? tenantId)
        {
            return await _context.Assignments
                .Where(a => a.ModuleId == moduleId
                            && a.TenantId == tenantId
                            && (entityId == null || a.ModuleId == moduleId || a.EntityId == entityId))
                .ToListAsync();
        }
        public async Task UpdateAsync(Assignment assignment)
        {
            _context.Assignments.Update(assignment);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
