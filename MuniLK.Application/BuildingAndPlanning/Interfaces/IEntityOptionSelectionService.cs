using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.Interfaces
{
    /// <summary>
    /// Service for managing entity option selections
    /// </summary>
    public interface IEntityOptionSelectionService
    {
        /// <summary>
        /// Save option selections for an entity (replaces existing selections)
        /// </summary>
        Task<bool> SaveSelectionsAsync(
            Guid entityId, 
            string entityType, 
            Guid moduleId, 
            List<Guid> optionItemIds, 
            CancellationToken ct = default);

        /// <summary>
        /// Get saved option selections for an entity
        /// </summary>
        Task<List<Guid>> GetSelectionsAsync(
            Guid entityId, 
            string entityType, 
            Guid moduleId, 
            CancellationToken ct = default);
    }
}
