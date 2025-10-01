// MuniLK.Infrastructure/Configurations/LookupConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuniLK.Domain.Entities; // For Lookup and LookupCategory

namespace MuniLK.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the Entity Framework Core mapping for the Lookup entity.
    /// </summary>
    public class LookupConfiguration : IEntityTypeConfiguration<Lookup>
    {
        public void Configure(EntityTypeBuilder<Lookup> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Id)
                   .ValueGeneratedOnAdd();

            // Configure foreign key to LookupCategory
            builder.HasOne(l => l.LookupCategory) // Navigation property
                   .WithMany() // LookupCategory can have many Lookups
                   .HasForeignKey(l => l.LookupCategoryId) // Foreign key property
                   .IsRequired() // Lookup must belong to a category
                   .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete of category if lookup is deleted

            builder.Property(l => l.Value)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(l => l.Order)
                   .IsRequired();

            builder.Property(l => l.TenantId)
                   .IsRequired(false); // TenantId is nullable

            builder.Property(l => l.IsActive)
                   .IsRequired();

            // Unique index to enforce uniqueness per category, value, and tenant.
            builder.HasIndex(l => new { l.LookupCategoryId, l.Value, l.TenantId })
                   .IsUnique();
        }
    }
}
