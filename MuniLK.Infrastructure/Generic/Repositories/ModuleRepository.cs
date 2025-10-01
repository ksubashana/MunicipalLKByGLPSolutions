using Microsoft.EntityFrameworkCore;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MuniLK.Infrastructure.Generic.Repositories
{
    public class ModuleRepository : IModuleRepository // No longer inheriting from GenericRepository
    {
        private readonly MuniLKDbContext _dbContext;

        public ModuleRepository(MuniLKDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Module> GetByIdAsync(Guid id)
        {
            return await _dbContext.Modules.FindAsync(id);
        }

        public async Task<IEnumerable<Module>> GetAllAsync()
        {
            return await _dbContext.Modules.ToListAsync();
        }

        public async Task<IEnumerable<Module>> FindAsync(Expression<Func<Module, bool>> predicate)
        {
            return await _dbContext.Modules.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(Module module)
        {
            await _dbContext.Modules.AddAsync(module);
        }

        public void Update(Module module)
        {
            _dbContext.Modules.Update(module);
        }

        public void Remove(Module module)
        {
            _dbContext.Modules.Remove(module);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        // Specific methods with eager loading
        public async Task<Module> GetModuleWithParentAsync(Guid id)
        {
            return await _dbContext.Modules
                                   .Include(m => m.ParentModule)
                                   .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<Module> GetModuleByCodeAsync(string code)
        {
            // Use FirstOrDefaultAsync to get the first module that matches the code.
            // We make sure to check if the code is case-insensitive by using ToUpper().
            return await _dbContext.Modules
                                   .FirstOrDefaultAsync(m => m.Code.ToUpper() == code.ToUpper());
        }
        public async Task<IEnumerable<Module>> GetAllModulesWithParentsAsync()
        {
            return await _dbContext.Modules
                                   .Include(m => m.ParentModule)
                                   .OrderBy(m => m.DisplayOrder)
                                   .ToListAsync();
        }
    }
}