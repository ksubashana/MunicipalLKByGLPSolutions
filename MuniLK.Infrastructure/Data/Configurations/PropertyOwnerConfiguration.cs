// MuniLK.Infrastructure/Data/Configurations/PropertyOwnerConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuniLK.Domain.Entities; // Adjust namespace

namespace MuniLK.Infrastructure.Data.Configurations
{
    public class PropertyOwnerConfiguration : IEntityTypeConfiguration<PropertyOwner>
    {
        public void Configure(EntityTypeBuilder<PropertyOwner> builder)
        {
            // Define the composite primary key
            builder.HasKey(po => new { po.PropertyId, po.ContactId });

            // Define relationships (these are also defined in ContactConfiguration and PropertyConfiguration,
            // but it's good practice to define both sides for clarity or if one side is optional)
            builder.HasOne(po => po.Property)
                   .WithMany(p => p.PropertyOwners)
                   .HasForeignKey(po => po.PropertyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(po => po.Contact)
                   .WithMany(c => c.PropertyOwners)
                   .HasForeignKey(po => po.ContactId)
                   .OnDelete(DeleteBehavior.Restrict);

            // You can add other property configurations for PropertyOwner here if needed
            builder.Property(po => po.OwnershipType)
                   .HasMaxLength(50); // Example
            builder.Property(po => po.PropertyId)
                  .ValueGeneratedOnAdd();

            builder.Property(po => po.PropertyId)
                   .IsRequired();

            builder.Property(po => po.TenantId)
                   .IsRequired();

            builder.Property(po => po.ContactId)
                   .IsRequired();
        }
    }
}