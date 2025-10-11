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

            builder.Property(eos => eos.OptionItemId)
                .IsRequired();

            builder.Property(eos => eos.ModuleId)
                .IsRequired();

            builder.Property(eos => eos.TenantId)
                .IsRequired(false);

            // Configure many-to-one relationship with OptionItem
            builder.HasOne(eos => eos.OptionItem)
                .WithMany()
                .HasForeignKey(eos => eos.OptionItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Create composite index for efficient queries by entity
            builder.HasIndex(eos => new { eos.EntityId, eos.EntityType, eos.ModuleId });
        }
    }
}
