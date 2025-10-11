using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuniLK.Domain.Entities;

namespace MuniLK.Infrastructure.Data.Configurations
{
    public class OptionGroupConfiguration : IEntityTypeConfiguration<OptionGroup>
    {
        public void Configure(EntityTypeBuilder<OptionGroup> builder)
        {
            builder.HasKey(og => og.Id);

            builder.Property(og => og.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(og => og.TenantId)
                .IsRequired(false);

            // Configure one-to-many relationship with OptionItem
            builder.HasMany(og => og.OptionItems)
                .WithOne(oi => oi.OptionGroup)
                .HasForeignKey(oi => oi.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
