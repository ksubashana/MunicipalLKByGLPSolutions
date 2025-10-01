using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.Generic.Result;

namespace MuniLK.Application.BuildingAndPlanning.Queries
{
    /// <summary>
    /// Query to get planning committee review by application ID
    /// </summary>
    public class GetPlanningCommitteeReviewQuery : IRequest<Result<PlanningCommitteeReviewResponse?>>
    {
        public Guid ApplicationId { get; set; }
    }
}