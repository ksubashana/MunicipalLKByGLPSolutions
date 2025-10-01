using MuniLK.Domain.Interfaces;
using System;

namespace MuniLK.Domain.Entities
{
    public class Assignment : IHasTenant
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }

        public Guid AssignedTo { get; set; }
        public Guid? AssignedBy { get; set; }

        public Guid? EntityId { get; set; }
        public string? EntityType { get; set; }

        public Guid ModuleId { get; set; }
        public Module Module { get; set; }

        public DateTime AssignmentDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string TaskType { get; set; } = string.Empty;
        public string? Notes { get; set; }

        // Outcome (optional, used when completing)
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedAt { get; set; }
        public string? Outcome { get; set; }
        public string? OutcomeRemarks { get; set; }

        // NEW: Feature ID (Building & Planning Application Number)
        public string? FeatureId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
