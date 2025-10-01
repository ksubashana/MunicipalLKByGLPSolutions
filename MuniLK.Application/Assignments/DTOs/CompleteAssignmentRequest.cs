using System;

namespace MuniLK.Application.Assignments.DTOs
{
    public class CompleteAssignmentRequest
    {
        public Guid AssignmentId { get; set; }
        // Examples: "Passed", "Failed", "Rejected"
        public string Outcome { get; set; } = string.Empty;
        public string? Remarks { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}