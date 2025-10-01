using MuniLK.Domain.Constants.Flows;

namespace MuniLK.Application.BuildingAndPlanning.DTOs
{
    /// <summary>
    /// DTO for requesting workflow step advancement
    /// </summary>
    public class AdvanceStepRequestDto
    {
        public Guid ApplicationId { get; set; }
        public ReviewDecision Decision { get; set; }
        public string? Comments { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public string PerformedByUserId { get; set; } = default!;
        public string PerformedByRole { get; set; } = default!;
    }
}