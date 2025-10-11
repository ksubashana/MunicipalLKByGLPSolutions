using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuniLK.Domain.Entities;

namespace MuniLK.Infrastructure.Data.Configurations
{
    public class OptionItemConfiguration : IEntityTypeConfiguration<OptionItem>
    {
        public void Configure(EntityTypeBuilder<OptionItem> builder)
        {
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.GroupId)
                .IsRequired();

            builder.Property(oi => oi.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(oi => oi.TenantId)
                .IsRequired(false);

            // Configure many-to-one relationship with OptionGroup
            builder.HasOne(oi => oi.OptionGroup)
                .WithMany(og => og.OptionItems)
                .HasForeignKey(oi => oi.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
