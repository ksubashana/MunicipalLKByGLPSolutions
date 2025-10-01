using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Domain.Entities
{
    public class WorkflowLog
    {
        public Guid Id { get; set; }

        public Guid TenantId { get; set; }

        public Guid ApplicationId { get; set; } // e.g., BuildingPlanApplicationId
        public string ActionTaken { get; set; } = default!;
        public string? PreviousStatus { get; set; }
        public string NewStatus { get; set; } = default!;
        public string? Remarks { get; set; }

        public string PerformedByUserId { get; set; } = default!;
        public string? PerformedByRole { get; set; }

        public string? AssignedToUserId { get; set; }
        public bool IsSystemGenerated { get; set; }

        public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
    }


}
