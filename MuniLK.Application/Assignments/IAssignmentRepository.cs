using MuniLK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Assignments
{
    public interface IAssignmentRepository
    {
        Task<Assignment> GetByIdAsync(Guid id);
        Task<IEnumerable<Assignment>> GetByInspectorAsync(Guid inspectorUserId, Guid tenantId);
        Task<IEnumerable<Assignment>> GetByModuleAndEntityAsync(Guid? moduleId, Guid? entityId, Guid? tenantId); // New method

        Task AddAsync(Assignment assignment);
        Task UpdateAsync(Assignment assignment);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}
