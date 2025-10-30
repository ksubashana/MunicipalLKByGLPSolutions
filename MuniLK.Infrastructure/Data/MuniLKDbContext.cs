using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Domain.Constants;
using MuniLK.Domain.Entities;
using MuniLK.Domain.Interfaces;
using MuniLK.Infrastructure.Data.ModelBuildingExtensions;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace MuniLK.Infrastructure.Data
{
    public class MuniLKDbContext : DbContext, IUnitOfWork
    {
        private readonly ICurrentTenantService _currentTenantService;
        private IDbContextTransaction? _currentTx;

        public MuniLKDbContext(DbContextOptions<MuniLKDbContext> options, ICurrentTenantService currentTenantService)
            : base(options)
        {
            _currentTenantService = currentTenantService;
        }
        // --- Add this DbSet for your new Tenant entity ---
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<License> Licenses => Set<License>();
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<MuniLK.Domain.Entities.ContactEntities.Contact> Contacts => Set<MuniLK.Domain.Entities.ContactEntities.Contact>();
        public DbSet<PropertyOwner> PropertyOwners => Set<PropertyOwner>();
        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<Lookup> Lookups { get; set; }
        public DbSet<LookupCategory> LookupCategories { get; set; } 
        public DbSet<ClientConfiguration> ClientConfigurations { get; set; }
        public DbSet<MuniLK.Domain.Entities.Document> Documents { get; set; }
        public DbSet<LicenseDocument> licenseDocuments { get; set; }
        public DbSet<DocumentLink> DocumentLinks { get; set; }
        public DbSet<BuildingPlanApplicationDocument> buildingPlanApplicationDocuments { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<BuildingPlanApplication> buildingPlanApplications { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<FeatureIdAudit> FeatureIdAudit { get; set; }
        public DbSet<WorkflowLog> WorkflowLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<ScheduleAppointments> ScheduleAppointments { get; set; }
        public DbSet<SiteInspection> SiteInspections { get; set; }
        public DbSet<PlanningCommitteeReview> PlanningCommitteeReviews { get; set; }
        public DbSet<EntityOptionSelection> EntityOptionSelections { get; set; }
        public DbSet<PlanningCommitteeMeeting> PlanningCommitteeMeetings { get; set; }
        public DbSet<PlanningCommitteeMeetingMember> PlanningCommitteeMeetingMembers { get; set; }
        public DbSet<PlanningCommitteeMeetingApplication> PlanningCommitteeMeetingApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Apply all IEntityTypeConfiguration classes from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MuniLKDbContext).Assembly);

            // Apply the tenant-specific query filter using your extension method
            modelBuilder.ApplyTenantFilters(_currentTenantService);
            //Apply global query filter for multi - tenancy
        }

        // Override SaveChanges to automatically set TenantId for new IHasTenant entities
        public override int SaveChanges()
        {
            SetTenantIdForNewEntities();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTenantIdForNewEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }
        public async Task<IDisposable> BeginTransactionAsync(CancellationToken ct = default)
        {
            _currentTx = await Database.BeginTransactionAsync(ct);
            return _currentTx;
        }

        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            if (_currentTx is null) return;
            await _currentTx.CommitAsync(ct);
            await _currentTx.DisposeAsync();
            _currentTx = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            if (_currentTx is null) return;
            await _currentTx.RollbackAsync(ct);
            await _currentTx.DisposeAsync();
            _currentTx = null;
        }
        private void SetTenantIdForNewEntities()
        {
            var tenantId = _currentTenantService.GetTenantId();

            if (!tenantId.HasValue)
            {
                // For entities that MUST have a TenantId, you might throw an exception here.
                // For instance, if you're saving a new License or ClientConfiguration,
                // the TenantId *must* be known.
                // throw new InvalidOperationException("Tenant ID must be set for tenant-scoped entities before saving.");
                return; // Or set a default system GUID if applicable for system-level data
            }

            foreach (var entry in ChangeTracker.Entries<IHasTenant>())
            {
                if (entry.State == EntityState.Added)
                {
                    // Ensure TenantId is not already set (e.g., if explicitly set by logic)
                    var currentTenantIdValue = (Guid)entry.Property(nameof(IHasTenant.TenantId)).CurrentValue;
                    if (currentTenantIdValue == Guid.Empty)
                    {
                        entry.Property(nameof(IHasTenant.TenantId)).CurrentValue = tenantId.Value;
                    }
                }
            }
            
        }
    }
}