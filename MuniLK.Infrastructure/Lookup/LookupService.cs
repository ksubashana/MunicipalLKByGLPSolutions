// MuniLK.Application/Services/LookupService.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MuniLK.Application.BuildingAndPlanning.DTOs; // Added for EntityOptionSelectionsResponse
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.Services.DTOs; // For DTOs
using MuniLK.Domain.Constants;
using MuniLK.Domain.Constants;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data; // Your DbContext
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuniLK.Application.Services
{
    /// <summary>
    /// Implements the ILookupService for managing and retrieving lookup values and categories.
    /// </summary>
    public class LookupService : ILookupService
    {
        private readonly MuniLKDbContext _context;
        private readonly ICurrentTenantService _currentTenantService;
        private readonly ILogger<LookupService> _logger;

        public LookupService(MuniLKDbContext context, ICurrentTenantService currentTenantService, ILogger<LookupService> logger)
        {
            _context = context;
            _currentTenantService = currentTenantService;
            _logger = logger;
        }

        /// <summary>
        /// Helper method to map Lookup entity to LookupValueDto.
        /// </summary>
        private LookupDto MapToLookupValueDto(Lookup lookup)
        {
            return new LookupDto
            {
                Id = lookup.Id,
                LookupCategoryId = lookup.LookupCategoryId,
                CategoryName = lookup.LookupCategory?.Name ?? "Unknown", // Include category name
                CategoryDisplayName = lookup.LookupCategory?.DisplayName ?? "Unknown", // Include category display name
                Value = lookup.Value,
                Order = lookup.Order,
                TenantId = lookup.TenantId,
                IsActive = lookup.IsActive
            };
        }

        /// <summary>
        /// Retrieves a list of active lookup values for a given category (by its programmatic name).
        /// </summary>
        public async Task<List<LookupDto>> GetLookupValuesByCategoryNameAsync(string categoryName)
        {
            var category = await _context.LookupCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(lc => lc.Name == categoryName && lc.IsActive);

            if (category == null)
            {
                _logger.LogWarning("Lookup category '{CategoryName}' not found.", categoryName);
                return new List<LookupDto>(); // Return empty list if category doesn't exist
            }

            return await GetLookupValuesByCategoryIdAsync(category.Id);
        }

        /// <summary>
        /// Retrieves a list of active lookup values for a given category (by its ID).
        /// </summary>
        public async Task<List<LookupDto>> GetLookupValuesByCategoryIdAsync(Guid lookupCategoryId)
        {
            var tenantId = _currentTenantService.GetTenantId();

            // Fetch global values for the category
            var globalValues = await _context.Lookups
                .AsNoTracking()
                .Include(l => l.LookupCategory) // Include category for DTO mapping
                .Where(l => l.LookupCategoryId == lookupCategoryId && l.TenantId == SystemConstants.SystemTenantId && l.IsActive)
                .OrderBy(l => l.Order)
                .ThenBy(l => l.Value)
                .ToListAsync();

            var finalValuesMap = new Dictionary<string, LookupDto>(); // Key: Value, Value: LookupValueDto

            foreach (var lv in globalValues)
            {
                finalValuesMap[lv.Value] = MapToLookupValueDto(lv);
            }

            if (tenantId.HasValue)
            {
                // Fetch tenant-specific values
                var tenantSpecificValues = await _context.Lookups
                    .AsNoTracking()
                    .Include(l => l.LookupCategory) // Include category for DTO mapping
                    .Where(l => l.LookupCategoryId == lookupCategoryId && l.TenantId == tenantId.Value && l.IsActive)
                    .OrderBy(l => l.Order)
                    .ThenBy(l => l.Value)
                    .ToListAsync();

                foreach (var lv in tenantSpecificValues)
                {
                    // Tenant-specific values will overwrite global ones if same 'Value'
                    finalValuesMap[lv.Value] = MapToLookupValueDto(lv);
                }
            }

            return finalValuesMap.Values.OrderBy(dto => dto.Order).ThenBy(dto => dto.Value).ToList();
        }

        /// <summary>
        /// Adds a new lookup value. Can be global or tenant-specific.
        /// </summary>
        public async Task<Guid> AddLookupValueAsync(AddLookupRequest request)
        {
            var tenantId = _currentTenantService.GetTenantId();

            if (!request.IsGlobal && !tenantId.HasValue)
            {
                throw new InvalidOperationException("Cannot add a tenant-specific lookup value without a current tenant context.");
            }

            // Validate LookupCategory exists
            var categoryExists = await _context.LookupCategories.IgnoreQueryFilters().AsNoTracking()
                .AnyAsync(lc => lc.Id == request.LookupCategoryId && lc.IsActive) ;
            if (!categoryExists)
            {
                throw new InvalidOperationException($"Lookup category with ID '{request.LookupCategoryId}' not found or is inactive.");
            }

            // Check for existing duplicate (global or tenant-specific)
            var existing = await _context.Lookups
                .AnyAsync(l => l.LookupCategoryId == request.LookupCategoryId && l.Value == request.Value &&
                               (request.IsGlobal ? l.TenantId == null : l.TenantId == tenantId.Value));

            if (existing)
            {
                throw new InvalidOperationException($"Lookup value '{request.Value}' already exists for the selected category {(request.IsGlobal ? "globally" : $"for tenant {tenantId.Value}")}.");
            }

            var newLookup = new Lookup
            {
                Id = Guid.NewGuid(),
                LookupCategoryId = request.LookupCategoryId,
                Value = request.Value,
                Order = request.Order,
                TenantId = request.IsGlobal ? SystemConstants.SystemTenantId : tenantId.Value,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _currentTenantService.GetTenantId()?.ToString() // Or actual user ID
            };

            _context.Lookups.Add(newLookup);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Added new lookup value: CategoryId='{CategoryId}', Value='{Value}', TenantId='{TenantId}'",
                request.LookupCategoryId, request.Value, newLookup.TenantId);

            return newLookup.Id;
        }

        public async Task<List<Guid>> AddLookupValuesBatchAsync(IEnumerable<AddLookupRequest> requests)
        {
            var newLookupIds = new List<Guid>();
            var tenantId = _currentTenantService.GetTenantId(); // Get current tenant ID

            foreach (var request in requests)
            {
                if (!request.IsGlobal && !tenantId.HasValue)
                {
                    throw new InvalidOperationException("Cannot add a tenant-specific lookup value without a current tenant context.");
                }

                // Validate LookupCategory exists
                var categoryExists = await _context.LookupCategories.IgnoreQueryFilters().AsNoTracking()
                    .AnyAsync(lc => lc.Id == request.LookupCategoryId && lc.IsActive);
                if (!categoryExists)
                {
                    throw new InvalidOperationException($"Lookup category with ID '{request.LookupCategoryId}' not found or is inactive.");
                }

                // Check for existing duplicate (global or tenant-specific)
                var existing = await _context.Lookups
                    .AnyAsync(l => l.LookupCategoryId == request.LookupCategoryId && l.Value == request.Value &&
                                   (request.IsGlobal ? l.TenantId == null : l.TenantId == tenantId.Value));

                if (existing)
                {
                    throw new InvalidOperationException($"Lookup value '{request.Value}' already exists for the selected category {(request.IsGlobal ? "globally" : $"for tenant {tenantId.Value}")}.");
                }

                var newLookup = new Lookup
                {
                    Id = Guid.NewGuid(),
                    LookupCategoryId = request.LookupCategoryId,
                    Value = request.Value,
                    Order = request.Order,
                    TenantId = request.IsGlobal ?  SystemConstants.SystemTenantId : tenantId.Value,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = _currentTenantService.GetTenantId()?.ToString() // Or actual user ID
                };

                _context.Lookups.Add(newLookup);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Added new lookup value: CategoryId='{CategoryId}', Value='{Value}', TenantId='{TenantId}'",
                    request.LookupCategoryId, request.Value, newLookup.TenantId);

                newLookupIds.Add(newLookup.Id);
            }

            // Save all changes in a single transaction
            await _context.SaveChangesAsync();

            return newLookupIds;
        }
        /// <summary>
        /// Retrieves all active lookup categories, combining global and tenant-specific ones.
        /// </summary>
        public async Task<List<LookupCategoryDto>> GetLookupCategoriesAsync()
        {
            var tenantId = _currentTenantService.GetTenantId();

            var categories = await _context.LookupCategories
                .AsNoTracking()
                .Where(lc => lc.IsActive && (lc.TenantId == SystemConstants.SystemTenantId || lc.TenantId == tenantId))
                .OrderBy(lc => lc.Order)
                .ThenBy(lc => lc.DisplayName)
                .Select(lc => new LookupCategoryDto
                {
                    Id = lc.Id,
                    Name = lc.Name,
                    DisplayName = lc.DisplayName,
                    Description = lc.Description,
                    Order = lc.Order,
                    TenantId = lc.TenantId,
                    IsActive = lc.IsActive
                })
                .ToListAsync();

            // Handle potential overrides for categories if a tenant has a category with the same 'Name' as a global one
            var finalCategoriesMap = new Dictionary<string, LookupCategoryDto>();
            foreach (var cat in categories.Where(c => c.TenantId == SystemConstants.SystemTenantId)) // Global categories first
            {
                finalCategoriesMap[cat.Name] = cat;
            }
            foreach (var cat in categories.Where(c => c.TenantId == tenantId)) // Tenant-specific categories override
            {
                finalCategoriesMap[cat.Name] = cat;
            }

            return finalCategoriesMap.Values.OrderBy(dto => dto.Order).ThenBy(dto => dto.DisplayName).ToList();
        }


        public async Task<Guid?> GetLookupCategoryIdByNameAsync(string name)
        {
            var tenantId = _currentTenantService.GetTenantId();

            // Fetch categories (active + global or tenant-specific)
            var categories = await _context.LookupCategories
                .AsNoTracking()
                .Where(lc => lc.IsActive && (lc.TenantId == SystemConstants.SystemTenantId || lc.TenantId == tenantId))
                .ToListAsync();

            // Tenant-specific overrides global
            var tenantCategory = categories.FirstOrDefault(c => c.TenantId == tenantId && c.Name == name);
            if (tenantCategory != null)
                return tenantCategory.Id;

            var globalCategory = categories.FirstOrDefault(c => c.TenantId == SystemConstants.SystemTenantId && c.Name == name);
            return globalCategory?.Id;
        }

        public async Task<Guid?> GetLookupIdByCategoryIdAndValueAsync(Guid lookupCategoryId, string value)
        {
            var tenantId = _currentTenantService.GetTenantId();

            // First, try to find a tenant-specific lookup with the given category and value
            if (tenantId.HasValue)
            {
                var tenantLookup = await _context.Lookups
                    .AsNoTracking()
                    .Where(l => l.LookupCategoryId == lookupCategoryId && l.TenantId == tenantId.Value && l.Value == value && l.IsActive)
                    .Select(l => l.Id)
                    .FirstOrDefaultAsync();

                if (tenantLookup != Guid.Empty)
                {
                    return tenantLookup;
                }
            }

            // If no tenant-specific lookup is found, look for a global one
            var globalLookup = await _context.Lookups
                .AsNoTracking()
                .Where(l => l.LookupCategoryId == lookupCategoryId && l.TenantId == SystemConstants.SystemTenantId && l.Value == value && l.IsActive)
                .Select(l => l.Id)
                .FirstOrDefaultAsync();

            if (globalLookup != Guid.Empty)
            {
                return globalLookup;
            }

            // If neither is found, return null
            return null;
        }
        /// <summary>
        /// Adds a new lookup category. Can be global or tenant-specific.
        /// </summary>
        public async Task<Guid> AddLookupCategoryAsync(AddLookupCategoryRequest request)
        {
            var tenantId = _currentTenantService.GetTenantId();

            if (!request.IsGlobal && !tenantId.HasValue)
            {
                throw new InvalidOperationException("Cannot add a tenant-specific lookup category without a current tenant context.");
            }

            // Check for existing duplicate category name (global or tenant-specific)
            var existing = await _context.LookupCategories
                .AnyAsync(lc => lc.Name == request.Name &&
                               (request.IsGlobal ? lc.TenantId == null : lc.TenantId == tenantId.Value));

            if (existing)
            {
                throw new InvalidOperationException($"Lookup category with name '{request.Name}' already exists {(request.IsGlobal ? "globally" : $"for tenant {tenantId.Value}")}.");
            }

            var newCategory = new LookupCategory
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                DisplayName = request.DisplayName,
                Description = request.Description,
                Order = request.Order,
                TenantId = request.IsGlobal ? SystemConstants.SystemTenantId : tenantId.Value,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _currentTenantService.GetTenantId()?.ToString() // Or actual user ID
            };

            _context.LookupCategories.Add(newCategory);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Added new lookup category: Name='{Name}', DisplayName='{DisplayName}', TenantId='{TenantId}'",
                request.Name, request.DisplayName, newCategory.TenantId);

            return newCategory.Id;
        }

        /// <summary>
        /// Validates if a given Lookup ID is valid for a specific category and tenant context.
        /// </summary>
        public async Task<bool> IsValidLookupIdAsync(Guid lookupId, string categoryName)
        {
            var tenantId = _currentTenantService.GetTenantId();

            // First, find the category ID by name
            var lookupCategory = await _context.LookupCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(lc => lc.Name == categoryName && lc.IsActive);

            if (lookupCategory == null)
            {
                _logger.LogWarning("Validation failed: Lookup category '{CategoryName}' not found or inactive.", categoryName);
                return false; // Category itself doesn't exist
            }

            // Then, check if the lookupId exists within that category for the current tenant or globally
            var isValid = await _context.Lookups
                .AsNoTracking()
                .AnyAsync(l => l.Id == lookupId &&
                               l.LookupCategoryId == lookupCategory.Id &&
                               l.IsActive &&
                               (l.TenantId == null || l.TenantId == tenantId)); // Check both global and tenant-specific

            if (!isValid)
            {
                _logger.LogWarning("Validation failed: Lookup ID '{LookupId}' not found or inactive for category '{CategoryName}' and tenant '{TenantId}'.",
                    lookupId, categoryName, tenantId);
            }

            return isValid;
        }

       public async Task<string?> GetLookupValueForCategoryAsync(Guid lookupId, string lookupCategoryName)
        {
            var lookupValue = await _context.Lookups
                .Include(l => l.LookupCategory) // Include category to filter by its name
                .Where(l => l.Id == lookupId &&
                            l.LookupCategory.Name == lookupCategoryName &&
                            l.IsActive)
                .Select(l => l.Value)
                .FirstOrDefaultAsync();

            return lookupValue;
        }

        // NEW: Entity option selection operations
        public async Task<List<Guid>> GetEntityOptionSelectionsAsync(Guid entityId, string entityType, Guid moduleId)
        {
            return await _context.EntityOptionSelections
                .AsNoTracking()
                .Where(e => e.EntityId == entityId && e.EntityType == entityType && e.ModuleId == moduleId)
                .Select(e => e.LookupId ?? e.OptionItemId)
                .Distinct()
                .ToListAsync();
        }

        public async Task<EntityOptionSelectionsResponse> SaveEntityOptionSelectionsAsync(Guid entityId, string entityType, Guid moduleId, List<Guid> optionItemIds)
        {
            if (entityId == Guid.Empty) throw new ArgumentException("EntityId is required", nameof(entityId));
            if (string.IsNullOrWhiteSpace(entityType)) throw new ArgumentException("EntityType is required", nameof(entityType));
            if (moduleId == Guid.Empty) throw new ArgumentException("ModuleId is required", nameof(moduleId));
            var tenantId = _currentTenantService.GetTenantId();

            var existing = await _context.EntityOptionSelections
                .Where(e => e.EntityId == entityId && e.EntityType == entityType && e.ModuleId == moduleId)
                .ToListAsync();
            if (existing.Any())
            {
                _context.EntityOptionSelections.RemoveRange(existing);
            }
            if (optionItemIds != null && optionItemIds.Any())
            {
                var newRows = optionItemIds.Select(id => new EntityOptionSelection
                {
                    Id = Guid.NewGuid(),
                    EntityId = entityId,
                    EntityType = entityType,
                    ModuleId = moduleId,
                    TenantId = tenantId.Value,
                    OptionItemId = id,
                    LookupId = id
                }).ToList();
                await _context.EntityOptionSelections.AddRangeAsync(newRows);
            }
            await _context.SaveChangesAsync();
            return new EntityOptionSelectionsResponse
            {
                EntityId = entityId,
                EntityType = entityType,
                ModuleId = moduleId,
                SelectedOptionItemIds = optionItemIds ?? new List<Guid>(),
                Success = true,
                Message = "Selections saved successfully"
            };
        }

        public async Task DeleteEntityOptionSelectionsAsync(Guid entityId, string entityType, Guid moduleId)
        {
            var existing = await _context.EntityOptionSelections
                .Where(e => e.EntityId == entityId && e.EntityType == entityType && e.ModuleId == moduleId)
                .ToListAsync();
            if (existing.Any())
            {
                _context.EntityOptionSelections.RemoveRange(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}