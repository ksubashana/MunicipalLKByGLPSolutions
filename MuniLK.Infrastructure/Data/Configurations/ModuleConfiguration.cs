using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Infrastructure.Data.Configurations
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Domain.Entities.Module>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Module> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
            builder.Property(m => m.Code).IsRequired().HasMaxLength(50);
            builder.HasIndex(m => m.Code).IsUnique(); // Ensure module codes are unique

            builder.HasOne(m => m.ParentModule)
                  .WithMany(m => m.ChildModules)
                  .HasForeignKey(m => m.ParentModuleId)
                  .IsRequired(false) // ParentModuleId can be null for top-level modules
                  .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete of parent module
        }
    }
}
