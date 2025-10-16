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
        private readonly IEntityOptionSelectionRepository _optionSelectionRepository;

        public GetSiteInspectionQueryHandler(ISiteInspectionRepository repository, IEntityOptionSelectionRepository optionSelectionRepository)
        {
            _repository = repository;
            _optionSelectionRepository = optionSelectionRepository;
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

            // Deprecated legacy ClearancesRequired JSON ignored (migrated to EntityOptionSelection)

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

            // Load option selections (clearances) using generic table keyed by SiteInspection.Id
            var selections = await _optionSelectionRepository.GetSelectionsAsync(siteInspection.Id, "SiteInspection", siteInspection.ApplicationId, ct);
            if (selections.Any())
            {
                response.ClearanceOptionItemIds = selections.Select(s => s.OptionItemId).ToList();
            }
            return response;
        }
    }
}
