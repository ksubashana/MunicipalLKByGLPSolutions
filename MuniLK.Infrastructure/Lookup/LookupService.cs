// MuniLK.Application/Services/LookupService.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.Services.DTOs;
using MuniLK.Domain.Constants;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuniLK.Application.Services
{
    public class LookupService : ILookupService
    {
        private readonly MuniLKDbContext _context;
        private readonly ICurrentTenantService _currentTenantService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<LookupService> _logger;

        private static readonly Dictionary<string, string> AllowedParentCategoryMappings = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Districts", "Provinces" },
            { "GSDivision", "Districts" }
        };

        public LookupService(MuniLKDbContext context, ICurrentTenantService currentTenantService, ICurrentUserService currentUserService, ILogger<LookupService> logger)
        {
            _context = context;
            _currentTenantService = currentTenantService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        private static LookupDto ToDto(Lookup entity) => new()
        {
            Id = entity.Id,
            LookupCategoryId = entity.LookupCategoryId,
            CategoryName = entity.LookupCategory?.Name ?? string.Empty,
            CategoryDisplayName = entity.LookupCategory?.DisplayName ?? string.Empty,
            Value = entity.Value,
            Order = entity.Order,
            TenantId = entity.TenantId,
            IsActive = entity.IsActive,
            ParentLookupId = entity.ParentLookupId,
            HasChildren = entity.Children != null && entity.Children.Any(c => c.IsActive)
        };

        public async Task<List<LookupDto>> GetLookupValuesByCategoryNameAsync(string categoryName)
        {
            var category = await _context.LookupCategories.AsNoTracking().FirstOrDefaultAsync(c => c.Name == categoryName && c.IsActive);
            if (category == null)
            {
                _logger.LogWarning("Lookup category '{CategoryName}' not found.", categoryName);
                return new List<LookupDto>();
            }
            return await GetLookupValuesByCategoryIdAsync(category.Id);
        }

        public async Task<List<LookupDto>> GetLookupValuesByCategoryIdAsync(Guid lookupCategoryId)
        {
            var tenantId = _currentTenantService.GetTenantId();
            var items = await _context.Lookups.AsNoTracking()
                .Include(x => x.LookupCategory)
                .Include(x => x.Children)
                .Where(x => x.LookupCategoryId == lookupCategoryId && x.IsActive && (x.TenantId == SystemConstants.SystemTenantId || x.TenantId == tenantId))
                .ToListAsync();

            var bestByValue = new Dictionary<string, Lookup>(StringComparer.OrdinalIgnoreCase);
            foreach (var g in items.Where(x => x.TenantId == SystemConstants.SystemTenantId)) bestByValue[g.Value] = g;
            foreach (var t in items.Where(x => x.TenantId == tenantId)) bestByValue[t.Value] = t; // override with tenant-specific
            return bestByValue.Values.OrderBy(x => x.Order).ThenBy(x => x.Value).Select(ToDto).ToList();
        }

        public async Task<Guid> AddLookupValueAsync(AddLookupRequest request)
        {
            var tenantId = _currentTenantService.GetTenantId();
            if (!request.IsGlobal && !tenantId.HasValue)
                throw new InvalidOperationException("Cannot add a tenant-specific lookup value without a tenant context.");

            var category = await _context.LookupCategories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.LookupCategoryId && c.IsActive);
            if (category == null)
                throw new InvalidOperationException($"Lookup category '{request.LookupCategoryId}' not found or inactive.");

            _logger.LogDebug("AddLookupValue: Category resolved as {CategoryName} (Id: {CategoryId})", category.Name, category.Id);

            if (request.ParentLookupId.HasValue)
            {
                var parent = await _context.Lookups.Include(p => p.LookupCategory).FirstOrDefaultAsync(p => p.Id == request.ParentLookupId.Value && p.IsActive);
                if (parent == null) throw new InvalidOperationException("Parent lookup not found or inactive.");

                _logger.LogDebug("AddLookupValue: Parent lookup category = {ParentCategoryName}", parent.LookupCategory?.Name);

                if (AllowedParentCategoryMappings.TryGetValue(category.Name.Trim(), out var requiredParentCategory))
                {
                    _logger.LogDebug("Mapping found for child category {ChildCategory}: required parent category {RequiredParentCategory}", category.Name, requiredParentCategory);
                    if (!string.Equals(parent.LookupCategory?.Name?.Trim(), requiredParentCategory, StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException($"Parent lookup must belong to category '{requiredParentCategory}'.");
                }
                else if (parent.LookupCategoryId != category.Id)
                {
                    _logger.LogDebug("No mapping for category {ChildCategory}; enforcing same-category rule.", category.Name);
                    throw new InvalidOperationException("Parent lookup must belong to the same category.");
                }
            }

            var exists = await _context.Lookups.AnyAsync(x => x.LookupCategoryId == request.LookupCategoryId && x.Value == request.Value && (request.IsGlobal ? x.TenantId == SystemConstants.SystemTenantId : x.TenantId == tenantId));
            if (exists) throw new InvalidOperationException("Lookup value already exists.");

            var createdBy = _currentUserService.UserId ?? _currentUserService.UserName ?? tenantId?.ToString();

            var entity = new Lookup
            {
                Id = Guid.NewGuid(),
                LookupCategoryId = request.LookupCategoryId,
                Value = request.Value,
                Order = request.Order,
                ParentLookupId = request.ParentLookupId,
                TenantId = request.IsGlobal ? SystemConstants.SystemTenantId : tenantId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            _context.Lookups.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<List<Guid>> AddLookupValuesBatchAsync(IEnumerable<AddLookupRequest> requests)
        {
            var result = new List<Guid>();
            foreach (var r in requests) result.Add(await AddLookupValueAsync(r));
            return result;
        }

        public async Task<List<LookupCategoryDto>> GetLookupCategoriesAsync()
        {
            var tenantId = _currentTenantService.GetTenantId();
            var categories = await _context.LookupCategories.AsNoTracking()
                .Where(c => c.IsActive && (c.TenantId == SystemConstants.SystemTenantId || c.TenantId == tenantId))
                .OrderBy(c => c.Order).ThenBy(c => c.DisplayName)
                .ToListAsync();

            var bestByName = new Dictionary<string, LookupCategory>(StringComparer.OrdinalIgnoreCase);
            foreach (var g in categories.Where(c => c.TenantId == SystemConstants.SystemTenantId)) bestByName[g.Name] = g;
            foreach (var t in categories.Where(c => c.TenantId == tenantId)) bestByName[t.Name] = t;

            return bestByName.Values.OrderBy(v => v.Order).ThenBy(v => v.DisplayName)
                .Select(c => new LookupCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    DisplayName = c.DisplayName,
                    Description = c.Description,
                    Order = c.Order,
                    TenantId = c.TenantId,
                    IsActive = c.IsActive
                }).ToList();
        }

        public async Task<Guid?> GetLookupCategoryIdByNameAsync(string name)
        {
            var tenantId = _currentTenantService.GetTenantId();
            var list = await _context.LookupCategories.AsNoTracking().Where(c => c.IsActive && (c.TenantId == SystemConstants.SystemTenantId || c.TenantId == tenantId)).ToListAsync();
            var tenantMatch = list.FirstOrDefault(c => c.TenantId == tenantId && c.Name == name);
            if (tenantMatch != null) return tenantMatch.Id;
            var globalMatch = list.FirstOrDefault(c => c.TenantId == SystemConstants.SystemTenantId && c.Name == name);
            return globalMatch?.Id;
        }

        public async Task<Guid?> GetLookupIdByCategoryIdAndValueAsync(Guid lookupCategoryId, string value)
        {
            var tenantId = _currentTenantService.GetTenantId();
            if (tenantId.HasValue)
            {
                var tenantIdMatch = await _context.Lookups.AsNoTracking().Where(x => x.LookupCategoryId == lookupCategoryId && x.TenantId == tenantId && x.Value == value && x.IsActive).Select(x => x.Id).FirstOrDefaultAsync();
                if (tenantIdMatch != Guid.Empty) return tenantIdMatch;
            }
            var globalMatch = await _context.Lookups.AsNoTracking().Where(x => x.LookupCategoryId == lookupCategoryId && x.TenantId == SystemConstants.SystemTenantId && x.Value == value && x.IsActive).Select(x => x.Id).FirstOrDefaultAsync();
            return globalMatch == Guid.Empty ? null : globalMatch;
        }

        public async Task<Guid> AddLookupCategoryAsync(AddLookupCategoryRequest request)
        {
            var tenantId = _currentTenantService.GetTenantId();
            if (!request.IsGlobal && !tenantId.HasValue) throw new InvalidOperationException("Cannot add tenant-specific category without tenant context.");
            var exists = await _context.LookupCategories.AnyAsync(c => c.Name == request.Name && (request.IsGlobal ? c.TenantId == SystemConstants.SystemTenantId : c.TenantId == tenantId));
            if (exists) throw new InvalidOperationException("Lookup category already exists.");
            var createdBy = _currentUserService.UserId ?? _currentUserService.UserName ?? tenantId?.ToString();
            var cat = new LookupCategory
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                DisplayName = request.DisplayName,
                Description = request.Description,
                Order = request.Order,
                TenantId = request.IsGlobal ? SystemConstants.SystemTenantId : tenantId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = createdBy
            };
            _context.LookupCategories.Add(cat);
            await _context.SaveChangesAsync();
            return cat.Id;
        }

        public async Task<bool> IsValidLookupIdAsync(Guid lookupId, string categoryName)
        {
            var tenantId = _currentTenantService.GetTenantId();
            var category = await _context.LookupCategories.AsNoTracking().FirstOrDefaultAsync(c => c.Name == categoryName && c.IsActive);
            if (category == null) return false;
            return await _context.Lookups.AsNoTracking().AnyAsync(x => x.Id == lookupId && x.LookupCategoryId == category.Id && x.IsActive && (x.TenantId == SystemConstants.SystemTenantId || x.TenantId == tenantId));
        }

        public async Task<string?> GetLookupValueForCategoryAsync(Guid lookupId, string lookupCategoryName)
        {
            return await _context.Lookups.Include(x => x.LookupCategory).Where(x => x.Id == lookupId && x.LookupCategory.Name == lookupCategoryName && x.IsActive).Select(x => x.Value).FirstOrDefaultAsync();
        }

        public async Task<List<Guid>> GetEntityOptionSelectionsAsync(Guid entityId, string entityType, Guid moduleId)
        {
            return await _context.EntityOptionSelections.AsNoTracking().Where(e => e.EntityId == entityId && e.EntityType == entityType && e.ModuleId == moduleId).Select(e => e.LookupId ?? e.OptionItemId).Distinct().ToListAsync();
        }

        public async Task<EntityOptionSelectionsResponse> SaveEntityOptionSelectionsAsync(Guid entityId, string entityType, Guid moduleId, List<Guid> optionItemIds)
        {
            if (entityId == Guid.Empty) throw new ArgumentException("EntityId is required", nameof(entityId));
            if (string.IsNullOrWhiteSpace(entityType)) throw new ArgumentException("EntityType is required", nameof(entityType));
            if (moduleId == Guid.Empty) throw new ArgumentException("ModuleId is required", nameof(moduleId));
            var tenantId = _currentTenantService.GetTenantId();
            var existing = await _context.EntityOptionSelections.Where(e => e.EntityId == entityId && e.EntityType == entityType && e.ModuleId == moduleId).ToListAsync();
            if (existing.Any()) _context.EntityOptionSelections.RemoveRange(existing);
            if (optionItemIds != null && optionItemIds.Any())
            {
                var rows = optionItemIds.Select(id => new EntityOptionSelection
                {
                    Id = Guid.NewGuid(),
                    EntityId = entityId,
                    EntityType = entityType,
                    ModuleId = moduleId,
                    TenantId = tenantId!.Value,
                    OptionItemId = id,
                    LookupId = id
                }).ToList();
                await _context.EntityOptionSelections.AddRangeAsync(rows);
            }
            await _context.SaveChangesAsync();
            return new EntityOptionSelectionsResponse { EntityId = entityId, EntityType = entityType, ModuleId = moduleId, SelectedOptionItemIds = optionItemIds ?? new List<Guid>(), Success = true, Message = "Selections saved successfully" };
        }

        public async Task DeleteEntityOptionSelectionsAsync(Guid entityId, string entityType, Guid moduleId)
        {
            var existing = await _context.EntityOptionSelections.Where(e => e.EntityId == entityId && e.EntityType == entityType && e.ModuleId == moduleId).ToListAsync();
            if (existing.Any())
            {
                _context.EntityOptionSelections.RemoveRange(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<LookupDto>> GetChildLookupsAsync(Guid parentLookupId)
        {
            var tenantId = _currentTenantService.GetTenantId();
            var children = await _context.Lookups.AsNoTracking().Include(x => x.LookupCategory).Include(x => x.Children)
                .Where(x => x.ParentLookupId == parentLookupId && x.IsActive && (x.TenantId == SystemConstants.SystemTenantId || x.TenantId == tenantId))
                .ToListAsync();
            return children.Select(ToDto).OrderBy(x => x.Order).ThenBy(x => x.Value).ToList();
        }

        public async Task<List<LookupDto>> GetRootLookupsByCategoryNameAsync(string categoryName)
        {
            var tenantId = _currentTenantService.GetTenantId();
            var category = await _context.LookupCategories.AsNoTracking().FirstOrDefaultAsync(c => c.Name == categoryName && c.IsActive);
            if (category == null) return new List<LookupDto>();
            var roots = await _context.Lookups.AsNoTracking().Include(x => x.LookupCategory).Include(x => x.Children)
                .Where(x => x.LookupCategoryId == category.Id && x.ParentLookupId == null && x.IsActive && (x.TenantId == SystemConstants.SystemTenantId || x.TenantId == tenantId))
                .ToListAsync();
            var bestByValue = new Dictionary<string, Lookup>(StringComparer.OrdinalIgnoreCase);
            foreach (var g in roots.Where(x => x.TenantId == SystemConstants.SystemTenantId)) bestByValue[g.Value] = g;
            foreach (var t in roots.Where(x => x.TenantId == tenantId)) bestByValue[t.Value] = t;
            return bestByValue.Values.OrderBy(x => x.Order).ThenBy(x => x.Value).Select(ToDto).ToList();
        }
    }
}