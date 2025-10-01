using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.Generic.Result;

namespace MuniLK.Application.BuildingAndPlanning.Commands
{
    /// <summary>
    /// Command to save/update planning committee review
    /// </summary>
    public class SavePlanningCommitteeReviewCommand : IRequest<Result<PlanningCommitteeReviewResponse>>
    {
        public PlanningCommitteeReviewRequest Request { get; set; } = default!;
        public string UserId { get; set; } = default!;
    }
}