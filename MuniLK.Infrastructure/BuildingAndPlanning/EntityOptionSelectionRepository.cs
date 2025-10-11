using Microsoft.EntityFrameworkCore;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Infrastructure.BuildingAndPlanning
{
    public class EntityOptionSelectionRepository : IEntityOptionSelectionRepository
    {
        private readonly MuniLKDbContext _context;

        public EntityOptionSelectionRepository(MuniLKDbContext context)
        {
            _context = context;
        }

        public async Task<List<EntityOptionSelection>> GetSelectionsAsync(
            Guid entityId, 
            string entityType, 
            Guid moduleId, 
            CancellationToken ct = default)
        {
            return await _context.EntityOptionSelections
                .Where(eos => eos.EntityId == entityId 
                    && eos.EntityType == entityType 
                    && eos.ModuleId == moduleId)
                .ToListAsync(ct);
        }

        public async Task AddSelectionsAsync(
            List<EntityOptionSelection> selections, 
            CancellationToken ct = default)
        {
            await _context.EntityOptionSelections.AddRangeAsync(selections, ct);
        }

        public async Task DeleteSelectionsAsync(
            Guid entityId, 
            string entityType, 
            Guid moduleId, 
            CancellationToken ct = default)
        {
            var existingSelections = await _context.EntityOptionSelections
                .Where(eos => eos.EntityId == entityId 
                    && eos.EntityType == entityType 
                    && eos.ModuleId == moduleId)
                .ToListAsync(ct);

            _context.EntityOptionSelections.RemoveRange(existingSelections);
        }

        public async Task<bool> ValidateOptionItemsExistAsync(
            List<Guid> optionItemIds, 
            CancellationToken ct = default)
        {
            if (optionItemIds == null || !optionItemIds.Any())
                return true;

            var existingCount = await _context.OptionItems
                .Where(oi => optionItemIds.Contains(oi.Id))
                .CountAsync(ct);

            return existingCount == optionItemIds.Count;
        }
    }
}
