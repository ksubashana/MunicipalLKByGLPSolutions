using System;
using System.Threading;
using System.Threading.Tasks;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MuniLK.Domain.Interfaces;
using System.Linq;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.Contact.DTOs;
using MuniLK.Application.PropertiesLK.DTOs;

namespace MuniLK.Infrastructure.BuildingAndPlanning
{
    public class BuildingAndPlanningRepository : IBuildingPlanRepository
    {
        private readonly MuniLKDbContext _dbContext;

        public BuildingAndPlanningRepository(MuniLKDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUnitOfWork UnitOfWork => _dbContext;

        public async Task AddAsync(BuildingPlanApplication entity, CancellationToken ct = default)
            => await _dbContext.Set<BuildingPlanApplication>().AddAsync(entity, ct);

        public async Task<BuildingPlanApplication?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _dbContext.Set<BuildingPlanApplication>()
                               .AsNoTracking()
                               .FirstOrDefaultAsync(x => x.Id == id, ct);
        public async Task<BuildingPlanApplication?> GetForUpdateAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.Set<BuildingPlanApplication>()
                .Include(a => a.WorkflowLogs)
                .FirstOrDefaultAsync(a => a.Id == id, ct);
        }
        public async Task<BuildingPlanApplication?> GetByIdWithWorkflowLogsAsync(Guid id, CancellationToken ct = default)
            => await _dbContext.Set<BuildingPlanApplication>()
                               .Include(x => x.WorkflowLogs)
                               .FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<List<WorkflowLog>> GetWorkflowLogsAsync(Guid applicationId, CancellationToken ct = default)
            => await _dbContext.WorkflowLogs
                               .Where(w => w.ApplicationId == applicationId)
                               .OrderBy(w => w.PerformedAt)
                               .ToListAsync(ct);

        // CHANGED: Now returns full BuildingPlanApplicationDto with summaries
        public List<BuildingPlanApplicationDto> SearchAsync(Guid? tenantId, CancellationToken ct = default)
        {
            var apps = _dbContext.Set<BuildingPlanApplication>()
                                 .AsNoTracking()
                                 .Include(a => a.Documents)
                                 .Include(a => a.WorkflowLogs)
                                 .AsQueryable();

            if (tenantId.HasValue)
                apps = apps.Where(a => a.TenantId == tenantId);

            // Joins for Applicant & Property basic info
            var query =
                from app in apps
                join contact in _dbContext.Contacts.AsNoTracking() on app.ApplicantContactId equals contact.Id
                join prop in _dbContext.Properties.AsNoTracking() on app.PropertyId equals prop.Id
                select new BuildingPlanApplicationDto
                {
                    Id = app.Id,
                    ApplicationNumber = app.ApplicationNumber,
                    Status = app.Status,
                    ApplicantContactId = app.ApplicantContactId,
                    ApplicantSummary = new ContactResponse
                    {
                        Id = contact.Id,
                        NationalId = contact.NIC,
                        FullName = contact.FullName,
                        Address = string.Join(" ", new[] { contact.AddressLine1, contact.AddressLine2 }.Where(s => !string.IsNullOrWhiteSpace(s))),
                        Email = contact.Email ?? string.Empty,
                        PhoneNumber = contact.PhoneNumber ?? string.Empty
                    },
                    PropertyId = app.PropertyId,
                    PropertySummary = new PropertyResponse
                    {
                        Id = prop.Id,
                        PropertyId = prop.PropertyId,
                        Address = prop.Address,
                        TitleDeedNumber = prop.TitleDeedNumber,
                        AssessmentValue = prop.AssessmentValue,
                        IsCommercialUse = prop.IsCommercialUse
                    },
                    BuildingPurpose = app.BuildingPurpose,
                    NoOfFloors = app.NoOfFloors,
                    ArchitectName = app.ArchitectName,
                    EngineerName = app.EngineerName,
                    Remarks = app.Remarks,
                    SubmittedOn = app.SubmittedOn,
                    ApprovedOn = app.ApprovedOn,
                    ApprovedByUserId = app.ApprovedByUserId,
                    PlanningReport = app.PlanningReport,
                    EngineerReport = app.EngineerReport,
                    CommissionerDecision = app.CommissionerDecision
                };

            return query.ToList();
        }

        public async Task<(List<BuildingPlanListItemDto> Items, int Total)> SearchListAsync(
    Guid? tenantId,
    int skip,
    int take,
    string? search,
    CancellationToken ct = default)
{
    // Base (tenant-scoped) applications
    var query =
        from app in _dbContext.buildingPlanApplications.AsNoTracking()
        where !tenantId.HasValue || app.TenantId == tenantId
        join c in _dbContext.Contacts.AsNoTracking()
            on app.ApplicantContactId equals c.Id into cgrp
        from contact in cgrp.DefaultIfEmpty() // LEFT JOIN Contact
        join p in _dbContext.Properties.AsNoTracking()
            on app.PropertyId equals p.Id into pgrp
        from prop in pgrp.DefaultIfEmpty() // LEFT JOIN Property
        select new
        {
            app.Id,
            app.ApplicationNumber,
            app.Status,
            ApplicantName = contact != null ? (contact.FullName ?? string.Empty) : string.Empty,
            ApplicantNIC = contact != null ? (contact.NIC ?? string.Empty) : string.Empty,
            PropertyAddress = prop != null ? (prop.Address ?? string.Empty) : string.Empty,
            app.BuildingPurpose,
            app.NoOfFloors,
            app.SubmittedOn
        };

    if (!string.IsNullOrWhiteSpace(search))
    {
        var term = $"%{search.Trim()}%";
        query = query.Where(x =>
            EF.Functions.Like(x.ApplicationNumber, term) ||
            EF.Functions.Like(x.ApplicantName, term) ||
            EF.Functions.Like(x.ApplicantNIC, term) ||
            EF.Functions.Like(x.PropertyAddress, term) ||
            EF.Functions.Like(x.BuildingPurpose, term));
    }

    var total = await query.CountAsync(ct);

    var page = await query
        .OrderByDescending(x => x.SubmittedOn)
        .Skip(skip)
        .Take(take)
        .Select(x => new BuildingPlanListItemDto
        {
            Id = x.Id,
            ApplicationNumber = x.ApplicationNumber,
            Status = x.Status,
            ApplicantName = x.ApplicantName,
            ApplicantNIC = x.ApplicantNIC,
            PropertyAddress = x.PropertyAddress,
            BuildingPurpose = x.BuildingPurpose,
            NoOfFloors = x.NoOfFloors,
            SubmittedOn = x.SubmittedOn
        })
        .ToListAsync(ct);

    return (page, total);
}
    }
}
