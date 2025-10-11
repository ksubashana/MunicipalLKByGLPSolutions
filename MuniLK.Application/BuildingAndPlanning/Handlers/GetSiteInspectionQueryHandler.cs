using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.BuildingAndPlanning.Queries;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.Handlers
{
    public class GetSiteInspectionQueryHandler : IRequestHandler<GetSiteInspectionQuery, SiteInspectionResponse?>
    {
        private readonly ISiteInspectionRepository _repository;

        public GetSiteInspectionQueryHandler(ISiteInspectionRepository repository)
        {
            _repository = repository;
        }

        public async Task<SiteInspectionResponse?> Handle(GetSiteInspectionQuery request, CancellationToken ct)
        {
            var siteInspection = await _repository.GetByInspectionIdAsync(request.InspectionId, ct);
            
            if (siteInspection == null)
                return null;

            // Map entity to response DTO
            var response = new SiteInspectionResponse
            {
                Id = siteInspection.Id,
                ApplicationId = siteInspection.ApplicationId,
                InspectionId = siteInspection.InspectionId,
                Status = siteInspection.Status,
                Remarks = siteInspection.Remarks,
                InspectionDate = siteInspection.InspectionDate,
                OfficersPresent = siteInspection.OfficersPresent,
                GpsCoordinates = siteInspection.GpsCoordinates,
                RequiredModifications = siteInspection.RequiredModifications,
                FinalRecommendation = siteInspection.FinalRecommendation,
                CreatedDate = siteInspection.CreatedDate,
                CreatedBy = siteInspection.CreatedBy,
                ModifiedDate = siteInspection.ModifiedDate,
                ModifiedBy = siteInspection.ModifiedBy
            };

            // Parse photos if available
            if (!string.IsNullOrEmpty(siteInspection.PhotosPaths))
            {
                try
                {
                    response.PhotoUrls = System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.List<string>>(siteInspection.PhotosPaths);
                }
                catch
                {
                    response.PhotoUrls = new System.Collections.Generic.List<string>();
                }
            }

            // Parse clearances if available
            if (!string.IsNullOrEmpty(siteInspection.ClearancesRequired))
            {
                try
                {
                    response.ClearancesRequired = System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.List<MuniLK.Domain.Constants.Flows.ClearanceType>>(siteInspection.ClearancesRequired);
                }
                catch
                {
                    response.ClearancesRequired = new System.Collections.Generic.List<MuniLK.Domain.Constants.Flows.ClearanceType>();
                }
            }

            // Map site conditions
            response.SiteConditions = new System.Collections.Generic.List<SiteConditionResult>
            {
                new SiteConditionResult { Name = "AccessRoadWidth", Result = siteInspection.AccessRoadWidthCondition, Notes = siteInspection.AccessRoadWidthNotes },
                new SiteConditionResult { Name = "BoundaryVerification", Result = siteInspection.BoundaryVerification, Notes = siteInspection.BoundaryVerificationNotes },
                new SiteConditionResult { Name = "Topography", Result = siteInspection.Topography, Notes = siteInspection.TopographyNotes },
                new SiteConditionResult { Name = "ExistingStructures", Result = siteInspection.ExistingStructures, Notes = siteInspection.ExistingStructuresNotes },
                new SiteConditionResult { Name = "EncroachmentsReservations", Result = siteInspection.EncroachmentsReservations, Notes = siteInspection.EncroachmentsReservationsNotes }
            };

            // Map compliance checks
            response.ComplianceChecks = new System.Collections.Generic.List<ComplianceCheckResult>
            {
                new ComplianceCheckResult { Name = "MatchesSurveyPlan", Result = siteInspection.MatchesSurveyPlan, Notes = siteInspection.MatchesSurveyPlanNotes },
                new ComplianceCheckResult { Name = "ZoningCompatible", Result = siteInspection.ZoningCompatible, Notes = siteInspection.ZoningCompatibleNotes },
                new ComplianceCheckResult { Name = "SetbacksObserved", Result = siteInspection.SetbacksObserved, Notes = siteInspection.SetbacksObservedNotes },
                new ComplianceCheckResult { Name = "EnvironmentalConcerns", Result = siteInspection.EnvironmentalConcerns, Notes = siteInspection.EnvironmentalConcernsNotes }
            };

            return response;
        }
    }
}
