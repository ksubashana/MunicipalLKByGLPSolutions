// MuniLK.Infrastructure/Data/Configurations/ClientConfigurationMapping.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuniLK.Domain.Entities; // Adjust namespace

namespace MuniLK.Infrastructure.Data.Configurations
{
    public class ClientConfigurationMapping : IEntityTypeConfiguration<ClientConfiguration>
    {
        public void Configure(EntityTypeBuilder<ClientConfiguration> builder)
        {
            builder.HasKey(c => c.Id); // Assuming Id is the primary key for ClientConfiguration

            // THIS IS THE SPECIFIC INDEX YOU ASKED ABOUT
            builder.HasIndex(c => new { c.TenantId, c.ConfigKey })
                   .IsUnique();

            // Other property configurations for ClientConfiguration if any
            builder.Property(c => c.ConfigKey).IsRequired().HasMaxLength(100);
            builder.Property(c => c.ConfigJson).IsRequired();
        }
    }
}