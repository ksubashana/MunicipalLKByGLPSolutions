using MuniLK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.Interfaces
{
    public interface IEntityOptionSelectionRepository
    {
        Task<List<EntityOptionSelection>> GetSelectionsAsync(
            Guid entityId, 
            string entityType, 
            Guid moduleId, 
            CancellationToken ct = default);

        Task AddSelectionsAsync(
            List<EntityOptionSelection> selections, 
            CancellationToken ct = default);

        Task DeleteSelectionsAsync(
            Guid entityId, 
            string entityType, 
            Guid moduleId, 
            CancellationToken ct = default);

        Task<bool> ValidateOptionItemsExistAsync(
            List<Guid> optionItemIds, 
            CancellationToken ct = default);
    }
}
