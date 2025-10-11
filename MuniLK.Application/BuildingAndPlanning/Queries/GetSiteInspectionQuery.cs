using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using System;

namespace MuniLK.Application.BuildingAndPlanning.Queries
{
    /// <summary>
    /// Query to get site inspection details by inspection ID
    /// </summary>
    public record GetSiteInspectionQuery(Guid InspectionId) : IRequest<SiteInspectionResponse?>;
}
