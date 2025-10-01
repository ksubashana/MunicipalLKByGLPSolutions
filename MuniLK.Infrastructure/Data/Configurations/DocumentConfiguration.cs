// MuniLK.Infrastructure/Data/Configurations/DocumentConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuniLK.Domain.Entities;

namespace MuniLK.Infrastructure.Data.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasKey(d => d.Id);

            // No ValueGeneratedOnAdd() for Guid Id if you generate them in the application (recommended)
            // builder.Property(d => d.Id).ValueGeneratedOnAdd(); // REMOVE THIS LINE if you use Guid.NewGuid()

            builder.Property(d => d.BlobPath)
                .IsRequired()
                .HasMaxLength(500); // Adjust length as per your blob path requirements

            builder.Property(d => d.FileName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(d => d.FileExtension)
                .IsRequired()
                .HasMaxLength(10); // e.g., ".pdf"

            builder.Property(d => d.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.FileSize)
                .IsRequired(); // long is non-nullable by default

            builder.Property(d => d.Description)
                .HasMaxLength(1000); // Nullable

            // Configure relationship with Lookup for DocumentType
            builder.HasOne(d => d.DocumentType)
                .WithMany() // No direct collection on Lookup to Document, it's a one-to-many from Document to Lookup
                .HasForeignKey(d => d.DocumentTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); // Prevent accidental deletion of a Lookup that's still referenced

            // Configure relationship with Lookup for DocumentStatus (optional)
            builder.HasOne(d => d.DocumentStatus)
                .WithMany()
                .HasForeignKey(d => d.DocumentStatusId)
                .IsRequired(false) // Status is optional
                .OnDelete(DeleteBehavior.Restrict);

            // Configure TenantId (already handled by IHasTenant filter, but good to ensure non-nullable if it must always be present)
            // If TenantId should be mandatory (not nullable Guid?), use .IsRequired()
            // builder.Property(d => d.TenantId).IsRequired(); // Only if your domain dictates TenantId CANNOT be null
        }
    }
}