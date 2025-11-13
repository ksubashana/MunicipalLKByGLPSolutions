using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.BuildingAndPlanning.Queries;
using MuniLK.Domain.Constants; // for LookupCategoryNames
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
            if (siteInspection == null) return null;

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

            if (!string.IsNullOrEmpty(siteInspection.PhotosPaths))
            {
                try
                {
                    response.PhotoUrls = System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.List<string>>(siteInspection.PhotosPaths) ?? new();
                }
                catch { response.PhotoUrls = new System.Collections.Generic.List<string>(); }
            }

            response.SiteConditions = new System.Collections.Generic.List<SiteConditionResult>
            {
                new() { Name = "AccessRoadWidth", Result = siteInspection.AccessRoadWidthCondition, Notes = siteInspection.AccessRoadWidthNotes },
                new() { Name = "BoundaryVerification", Result = siteInspection.BoundaryVerification, Notes = siteInspection.BoundaryVerificationNotes },
                new() { Name = "Topography", Result = siteInspection.Topography, Notes = siteInspection.TopographyNotes },
                new() { Name = "ExistingStructures", Result = siteInspection.ExistingStructures, Notes = siteInspection.ExistingStructuresNotes },
                new() { Name = "EncroachmentsReservations", Result = siteInspection.EncroachmentsReservations, Notes = siteInspection.EncroachmentsReservationsNotes }
            };

            response.ComplianceChecks = new System.Collections.Generic.List<ComplianceCheckResult>
            {
                new() { Name = "MatchesSurveyPlan", Result = siteInspection.MatchesSurveyPlan, Notes = siteInspection.MatchesSurveyPlanNotes },
                new() { Name = "ZoningCompatible", Result = siteInspection.ZoningCompatible, Notes = siteInspection.ZoningCompatibleNotes },
                new() { Name = "SetbacksObserved", Result = siteInspection.SetbacksObserved, Notes = siteInspection.SetbacksObservedNotes },
                new() { Name = "EnvironmentalConcerns", Result = siteInspection.EnvironmentalConcerns, Notes = siteInspection.EnvironmentalConcernsNotes }
            };

            // Load option selections (clearances) and map to response using new LookupId field
            var selections = await _optionSelectionRepository.GetSelectionsAsync(siteInspection.Id, "SiteInspection", siteInspection.ApplicationId, ct);
            if (selections.Any())
            {
                var clearanceIds = selections
                    .Where(s => string.Equals(s.LookupCategoryName, LookupCategoryNames.ClearanceTypes.ToString(), StringComparison.OrdinalIgnoreCase))
                    .Select(s => s.LookupId)
                    .ToList();
                if (clearanceIds.Any()) response.ClearanceOptionItemIds = clearanceIds;
            }
            return response;
        }
    }
}
