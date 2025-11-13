using System;
using System.Collections.Generic;

namespace MuniLK.Application.BuildingAndPlanning.DTOs
{
    /// <summary>
    /// Request DTO for saving entity option selections
    /// </summary>
    public class SaveEntityOptionSelectionsRequest
    {
        public Guid EntityId { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public Guid ModuleId { get; set; }
        public string LookupCategoryName { get; set; } = string.Empty; // new discriminator
        public List<Guid> OptionItemIds { get; set; } = new();
    }

    /// <summary>
    /// Response DTO for entity option selections
    /// </summary>
    public class EntityOptionSelectionsResponse
    {
        public Guid EntityId { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public Guid ModuleId { get; set; }
        public string LookupCategoryName { get; set; } = string.Empty; // new discriminator
        public List<Guid> SelectedOptionItemIds { get; set; } = new();
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
