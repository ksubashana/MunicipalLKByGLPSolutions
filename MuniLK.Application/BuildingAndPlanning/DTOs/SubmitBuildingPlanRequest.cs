using MuniLK.Application.Documents.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.DTOs
{
    public class SubmitBuildingPlanRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid ApplicantContactId { get; set; }
        public Guid PropertyId { get; set; }
        public string BuildingPurpose { get; set; } = default!;
        public int NoOfFloors { get; set; }
        public string? ArchitectName { get; set; }
        public string? EngineerName { get; set; }
        public string? Remarks { get; set; }
        public List<UploadDocumentRequest>? Documents { get; set; } = new();
    }
}
