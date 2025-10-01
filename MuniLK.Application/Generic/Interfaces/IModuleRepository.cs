using MuniLK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Generic.Interfaces
{
    public interface IModuleRepository
    {
        Task<Module> GetByIdAsync(Guid id);
        Task<IEnumerable<Module>> GetAllAsync();
        Task<IEnumerable<Module>> FindAsync(Expression<Func<Module, bool>> predicate);
        Task AddAsync(Module module);
        void Update(Module module);
        void Remove(Module module);
        Task<int> SaveChangesAsync(); // Added for explicit unit of work

        // Specific methods with eager loading
        Task<Module> GetModuleWithParentAsync(Guid id);
        Task<Module> GetModuleByCodeAsync(string code);

        Task<IEnumerable<Module>> GetAllModulesWithParentsAsync();
    }
}
