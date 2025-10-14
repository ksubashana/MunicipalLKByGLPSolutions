using Microsoft.EntityFrameworkCore;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;

namespace MuniLK.Infrastructure.BuildingAndPlanning
{
    public class SiteInspectionRepository : ISiteInspectionRepository
    {
        private readonly MuniLKDbContext _context;

        public SiteInspectionRepository(MuniLKDbContext context)
        {
            _context = context;
        }

        public async Task<SiteInspection> AddAsync(SiteInspection siteInspection, CancellationToken cancellationToken = default)
        {
            var result = await _context.SiteInspections.AddAsync(siteInspection, cancellationToken);
            return result.Entity;
        }

        public async Task<SiteInspection?> GetByInspectionIdAsync(Guid inspectionId, CancellationToken cancellationToken = default)
        {
            return await _context.SiteInspections
                .FirstOrDefaultAsync(s => s.InspectionId == inspectionId, cancellationToken);
        }

        public async Task<IEnumerable<SiteInspection>> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default)
        {
            return await _context.SiteInspections
                .Where(s => s.ApplicationId == applicationId)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<(SiteInspection Entity, bool Created)> AddOrUpdateByInspectionIdAsync(SiteInspection siteInspection, CancellationToken cancellationToken = default)
        {
            if (siteInspection.InspectionId == null)
            {
                // Treat null InspectionId as a create with new business key
                siteInspection.InspectionId = Guid.NewGuid();
            }

            var existing = await _context.SiteInspections
                .FirstOrDefaultAsync(s => s.InspectionId == siteInspection.InspectionId, cancellationToken);

            if (existing == null)
            {
                var added = await _context.SiteInspections.AddAsync(siteInspection, cancellationToken);
                return (added.Entity, true);
            }

            // Update scalar properties (only those that can change)
            existing.Status = siteInspection.Status;
            existing.Remarks = siteInspection.Remarks;
            existing.InspectionDate = siteInspection.InspectionDate;
            existing.OfficersPresent = siteInspection.OfficersPresent;
            existing.GpsCoordinates = siteInspection.GpsCoordinates;
            existing.AccessRoadWidthCondition = siteInspection.AccessRoadWidthCondition;
            existing.AccessRoadWidthNotes = siteInspection.AccessRoadWidthNotes;
            existing.BoundaryVerification = siteInspection.BoundaryVerification;
            existing.BoundaryVerificationNotes = siteInspection.BoundaryVerificationNotes;
            existing.Topography = siteInspection.Topography;
            existing.TopographyNotes = siteInspection.TopographyNotes;
            existing.ExistingStructures = siteInspection.ExistingStructures;
            existing.ExistingStructuresNotes = siteInspection.ExistingStructuresNotes;
            existing.EncroachmentsReservations = siteInspection.EncroachmentsReservations;
            existing.EncroachmentsReservationsNotes = siteInspection.EncroachmentsReservationsNotes;
            existing.MatchesSurveyPlan = siteInspection.MatchesSurveyPlan;
            existing.MatchesSurveyPlanNotes = siteInspection.MatchesSurveyPlanNotes;
            existing.ZoningCompatible = siteInspection.ZoningCompatible;
            existing.ZoningCompatibleNotes = siteInspection.ZoningCompatibleNotes;
            existing.SetbacksObserved = siteInspection.SetbacksObserved;
            existing.SetbacksObservedNotes = siteInspection.SetbacksObservedNotes;
            existing.FrontSetback = siteInspection.FrontSetback;
            existing.RearSetback = siteInspection.RearSetback;
            existing.SideSetbacks = siteInspection.SideSetbacks;
            existing.EnvironmentalConcerns = siteInspection.EnvironmentalConcerns;
            existing.EnvironmentalConcernsNotes = siteInspection.EnvironmentalConcernsNotes;
            existing.RequiredModifications = siteInspection.RequiredModifications;
            existing.ClearancesRequired = siteInspection.ClearancesRequired;
            existing.FinalRecommendation = siteInspection.FinalRecommendation;
            existing.ModifiedDate = DateTime.UtcNow;
            existing.ModifiedBy = siteInspection.ModifiedBy;

            return (existing, false);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changes = await _context.SaveChangesAsync(cancellationToken);
            return changes > 0;
        }
    }
}