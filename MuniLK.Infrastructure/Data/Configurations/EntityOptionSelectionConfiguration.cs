using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuniLK.Domain.Entities;

namespace MuniLK.Infrastructure.Data.Configurations
{
    public class EntityOptionSelectionConfiguration : IEntityTypeConfiguration<EntityOptionSelection>
    {
        public void Configure(EntityTypeBuilder<EntityOptionSelection> builder)
        {
            builder.HasKey(eos => eos.Id);

            builder.Property(eos => eos.EntityId)
                .IsRequired();

            builder.Property(eos => eos.EntityType)
                .IsRequired()
                .HasMaxLength(100);

            // Legacy OptionItemId no longer required
            builder.Property(eos => eos.OptionItemId)
                .IsRequired(false);

            // New LookupId (nullable during transition)
            builder.Property(eos => eos.LookupId)
                .IsRequired(false);

            builder.Property(eos => eos.ModuleId)
                .IsRequired();

            builder.Property(eos => eos.TenantId)
                .IsRequired(false);

            // Legacy relationship
            builder.HasOne(eos => eos.OptionItem)
                .WithMany()
                .HasForeignKey(eos => eos.OptionItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // New relationship to Lookup (if Lookup entity exists in model)
            builder.HasOne<Lookup>()
                .WithMany()
                .HasForeignKey(eos => eos.LookupId)
                .OnDelete(DeleteBehavior.Restrict);

            // Create composite index for efficient queries by entity
            builder.HasIndex(eos => new { eos.EntityId, eos.EntityType, eos.ModuleId });
        }
    }
}
