using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.DTOs
{
    public class BuildingPlanResponse
    {
        public Guid Id { get; set; }
        public string ApplicationNumber { get; set; } = default!;
        public Domain.Entities.Lookup Status { get; set; }
        public Guid ApplicantContactId { get; set; }
        public Guid PropertyId { get; set; }
        public string BuildingPurpose { get; set; } = default!;
        public int NoOfFloors { get; set; }
        public DateTime SubmittedOn { get; set; }
        public List<ApplicationDocumentResponse> Documents { get; set; } = new();
        public List<WorkflowLogResponse> Workflow { get; set; } = new();
        
        // Additional fields for grid display
        public string ApplicantName { get; set; } = default!;
        public string PropertyAddress { get; set; } = default!;
        public DateTime SubmittedDate => SubmittedOn; // Alias for consistency with requirements
    }
}
