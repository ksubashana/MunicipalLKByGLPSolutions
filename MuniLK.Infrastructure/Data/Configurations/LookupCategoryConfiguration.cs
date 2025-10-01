// MuniLK.Infrastructure/Configurations/LookupCategoryConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuniLK.Domain.Entities; // For LookupCategory

namespace MuniLK.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the Entity Framework Core mapping for the LookupCategory entity.
    /// </summary>
    public class LookupCategoryConfiguration : IEntityTypeConfiguration<LookupCategory>
    {
        public void Configure(EntityTypeBuilder<LookupCategory> builder)
        {
            builder.HasKey(lc => lc.Id);

            builder.Property(lc => lc.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(lc => lc.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(lc => lc.DisplayName)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(lc => lc.Description)
                   .HasMaxLength(500); // Nullable

            builder.Property(lc => lc.Order)
                   .IsRequired();

            builder.Property(lc => lc.TenantId)
                   .IsRequired(false); // TenantId is nullable

            builder.Property(lc => lc.IsActive)
                   .IsRequired();

            // Unique index for Name and TenantId to prevent duplicate categories (global or tenant-specific)
            builder.HasIndex(lc => new { lc.Name, lc.TenantId })
                   .IsUnique();
        }
    }
}
