using System;

namespace MuniLK.Application.Assignments.DTOs
{
    public class AssignmentResponse
    {
        public Guid Id { get; set; }

        // Assigned user
        public Guid AssignedToUserId { get; set; }
        public string AssignedToName { get; set; } = string.Empty;

        // Who assigned the task
        public Guid? AssignedByUserId { get; set; }
        public string AssignedByName { get; set; } = string.Empty;

        // Module info
        public Guid ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;

        // Linked entity info
        public Guid? EntityId { get; set; }
        public string? EntityType { get; set; }
        public string EntityReference { get; set; } = string.Empty;

        // Assignment details
        public DateTime AssignmentDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string TaskType { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }

        // Add the new fields already present on domain entity
        public string? Outcome { get; set; }
        public string? OutcomeRemarks { get; set; }
        public string? FeatureId { get; set; }
    }
}
