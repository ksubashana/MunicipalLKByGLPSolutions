using AutoMapper;
using MuniLK.Application.Generic.DTOs;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuniLK.Application.Services
{
    public class ModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IMapper _mapper;

        public ModuleService(IModuleRepository moduleRepository, IMapper mapper)
        {
            _moduleRepository = moduleRepository;
            _mapper = mapper;
        }

        // --- Module CRUD Operations ---

        public async Task<ModuleDto> CreateModuleAsync(ModuleCreateDto createDto)
        {
            var module = _mapper.Map<Module>(createDto);
            module.Id = Guid.NewGuid(); // Assign a new GUID

            // Basic validation: Check for unique code
            var existingModule = await _moduleRepository.FindAsync(m => m.Code == createDto.Code);
            if (existingModule.Any())
            {
                throw new InvalidOperationException($"Module with code '{createDto.Code}' already exists.");
            }

            await _moduleRepository.AddAsync(module);
            await _moduleRepository.SaveChangesAsync();

            return _mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDto> UpdateModuleAsync(ModuleUpdateDto updateDto)
        {
            var module = await _moduleRepository.GetByIdAsync(updateDto.Id);
            if (module == null)
            {
                return null; // Or throw NotFoundException (more robust error handling in real app)
            }

            // Basic validation for code change
            if (module.Code != updateDto.Code)
            {
                var existingModule = await _moduleRepository.FindAsync(m => m.Code == updateDto.Code && m.Id != updateDto.Id);
                if (existingModule.Any())
                {
                    throw new InvalidOperationException($"Module with code '{updateDto.Code}' already exists.");
                }
            }

            _mapper.Map(updateDto, module);
            _moduleRepository.Update(module);
            await _moduleRepository.SaveChangesAsync();

            return _mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDto> GetModuleByIdAsync(Guid id)
        {
            var module = await _moduleRepository.GetModuleWithParentAsync(id); // Use the specific method to include parent
            if (module == null)
            {
                return null;
            }
            return _mapper.Map<ModuleDto>(module);
        }
        public async Task<Guid?> GetModuleIdByCodeAsync(string code)
        {
            var module = await _moduleRepository.GetModuleByCodeAsync(code);

            // If a module is found, return its Id; otherwise, return null.
            return module?.Id;
        }
        public async Task<IEnumerable<ModuleDto>> GetAllModulesAsync()
        {
            var modules = await _moduleRepository.GetAllModulesWithParentsAsync(); // Use the specific method to include parents
            return _mapper.Map<IEnumerable<ModuleDto>>(modules);
        }

        public async Task DeleteModuleAsync(Guid id)
        {
            var module = await _moduleRepository.GetByIdAsync(id);
            if (module == null)
            {
                return; // Or throw NotFoundException
            }

            // Prevent deletion if the module is a core module
            if (module.IsCoreModule)
            {
                throw new InvalidOperationException("Core modules cannot be deleted.");
            }

            // Check if it's a parent module for others
            // (Note: This requires loading child modules or doing a separate query.
            // For simplicity, using FindAsync directly on ParentModuleId)
            var hasChildModules = await _moduleRepository.FindAsync(m => m.ParentModuleId == id);
            if (hasChildModules.Any())
            {
                throw new InvalidOperationException("Cannot delete module as it is a parent to other modules. Please reassign children first.");
            }

            // If you had other entities referencing Module (like Reports from previous discussion),
            // you'd add checks here using their respective repositories to prevent deletion if in use.
            // E.g., var hasReports = await _reportRepository.FindAsync(r => r.ModuleId == id);
            // if (hasReports.Any()) { throw new InvalidOperationException("Module has associated reports."); }

            _moduleRepository.Remove(module);
            await _moduleRepository.SaveChangesAsync();
        }
    }
}