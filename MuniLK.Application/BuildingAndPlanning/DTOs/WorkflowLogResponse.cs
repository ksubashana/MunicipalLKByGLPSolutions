using MuniLK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.DTOs
{
    public class WorkflowLogResponse
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Action { get; set; } = default!;
        public string? Remarks { get; set; }
        public string PerformedByUserId { get; set; } = default!;
        public DateTime PerformedOn { get; set; }
    }
}
