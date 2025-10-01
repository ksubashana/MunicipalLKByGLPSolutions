// MuniLK.Infrastructure/Data/ModelBuildingExtensions/TenantModelConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata; // For IModel
using MuniLK.Application.Generic.Interfaces; // For IHasTenant
using MuniLK.Domain.Constants;
using MuniLK.Domain.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MuniLK.Infrastructure.Data.ModelBuildingExtensions
{
    public static class TenantModelConfiguration
    {
        public static void ApplyTenantFilters(this ModelBuilder modelBuilder, ICurrentTenantService currentTenantService)
        {
            // You can also get the service from an IServiceProvider if you prefer,
            // but for simplicity here, we're passing it.
            // If currentTenantService is null during design-time (e.g., migrations), handle it gracefully.
            if (currentTenantService == null)
            {
                // This scenario often happens during design-time operations (migrations)
                // where the service provider isn't fully built.
                // For design-time, you might apply a filter that returns no data
                // or skip applying the filter if not critical for model building.
                // For now, we'll proceed assuming currentTenantService is available or TenantId can be default.
                // A better approach for design-time might involve a DesignTimeDbContextFactory
                // that provides a mock ICurrentTenantService.
            }

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                // Check if the entity implements IHasTenant and has a TenantId property
                if (typeof(IHasTenant).IsAssignableFrom(clrType) &&
                    entityType.FindProperty(nameof(IHasTenant.TenantId)) != null)
                {
                    var parameter = Expression.Parameter(clrType, "e");

                    // Get the current tenant ID from the service. This will be captured in the expression.
                    var tenantId = currentTenantService?.GetTenantId();
                    var tenantIdConstant = Expression.Constant(tenantId, typeof(Guid?));
                    var globalTenantConstant = Expression.Constant(SystemConstants.SystemTenantId, typeof(Guid?));

                    // Build expression: EF.Property<Guid?>(e, "TenantId")
                    var tenantProperty = Expression.Call(
                        typeof(EF),
                        nameof(EF.Property),
                        new[] { typeof(Guid?) },
                        parameter,
                        Expression.Constant(nameof(IHasTenant.TenantId))
                    );

                    // Build expression: EF.Property<Guid?>(e, "TenantId") == currentTenantService.GetTenantId()
                    // This works because the `tenantId` variable value at the time the expression is built
                    // (which is when the app starts up and OnModelCreating runs) is captured.

                    var tenantMatch = Expression.Equal(tenantProperty, tenantIdConstant);
                    var globalMatch = Expression.Equal(tenantProperty, globalTenantConstant);

                    var combined = Expression.OrElse(tenantMatch, globalMatch);

                    var lambda = Expression.Lambda(combined, parameter);

                    modelBuilder.Entity(clrType).HasQueryFilter(lambda);

                    // Optional: Add a unique index on (TenantId, PrimaryKey) for tenant-scoped entities
                    // This is for unique keys across tenants.
                    // This often requires careful consideration of existing primary keys.
                    // If your primary key is already composite (like PropertyOwner), this isn't needed for PK.
                    // If your entity has a single PK (e.g., Contact.Id), but you want (TenantId, Id) to be unique,
                    // you would add an index here.
                    /*
                    var primaryKeyProperty = entityType.FindPrimaryKey()?.Properties.FirstOrDefault();
                    if (primaryKeyProperty != null)
                    {
                        entityType.AddIndex(new[]
                        {
                            entityType.FindProperty(nameof(IHasTenant.TenantId)),
                            primaryKeyProperty
                        }).IsUnique();
                    }
                    */
                }
            }
        }
    }
}