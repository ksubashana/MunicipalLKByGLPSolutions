using System;

namespace MuniLK.Application.Assignments.DTOs
{
    public class CreateAssignmentRequest
    {
        public Guid AssignedToUserId { get; set; }
        public string AssignedToUser { get; set; }
        public Guid EntityId { get; set; }
        public string? EntityType { get; set; }
        public Guid ModuleId { get; set; }
        public DateTime AssignmentDate { get; set; }
        public string TaskType { get; set; } = "Inspection";
        public string? Notes { get; set; }

        // NEW: Feature ID (e.g., "BP/2025/000123")
        public string? FeatureId { get; set; }
    }
}
