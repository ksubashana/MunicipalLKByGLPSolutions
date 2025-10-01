using MuniLK.Domain.Constants.Flows;

namespace MuniLK.Application.BuildingAndPlanning.DTOs
{
    public class BuildingPlanListItemDto
    {
        public Guid Id { get; set; }
        public string ApplicationNumber { get; set; } = string.Empty;
        public BuildingAndPlanSteps Status { get; set; }
        public string StatusDisplay => Status.ToString(); // Replace with display attribute lookup if needed
        public string ApplicantName { get; set; } = string.Empty;
        public string ApplicantNIC { get; set; } = string.Empty;
        public string PropertyAddress { get; set; } = string.Empty;
        public string BuildingPurpose { get; set; } = string.Empty;
        public int NoOfFloors { get; set; }
        public DateTime SubmittedOn { get; set; }
    }
}