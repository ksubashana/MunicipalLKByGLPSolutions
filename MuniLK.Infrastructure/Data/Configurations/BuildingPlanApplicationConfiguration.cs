using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuniLK.Domain.Entities;

namespace MuniLK.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for BuildingPlanApplication
    /// </summary>
    public class BuildingPlanApplicationConfiguration : IEntityTypeConfiguration<BuildingPlanApplication>
    {
        public void Configure(EntityTypeBuilder<BuildingPlanApplication> builder)
        {
            builder.ToTable("buildingPlanApplications");
            
            builder.HasKey(x => x.Id);

            // Configure properties
            builder.Property(x => x.ApplicationNumber)
                .HasMaxLength(50);

            builder.Property(x => x.BuildingPurpose)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.ArchitectName)
                .HasMaxLength(200);

            builder.Property(x => x.EngineerName)
                .HasMaxLength(200);

            builder.Property(x => x.Remarks)
                .HasMaxLength(2000);

            builder.Property(x => x.ApprovedByUserId)
                .HasMaxLength(450);

            // Workflow-specific report fields
            builder.Property(x => x.PlanningReport)
                .HasMaxLength(4000);

            builder.Property(x => x.EngineerReport)
                .HasMaxLength(4000);

            builder.Property(x => x.CommissionerDecision)
                .HasMaxLength(4000);

            // Workflow child entity foreign keys (optional)
            builder.Property(x => x.AssignmentId)
                .IsRequired(false);

            builder.Property(x => x.SiteInspectionId)
                .IsRequired(false);

            builder.Property(x => x.PlanningCommitteeReviewId)
                .IsRequired(false);

            // Configure enum as int
            builder.Property(x => x.Status)
                .HasConversion<int>();

            // Configure navigation properties
            builder.HasMany(x => x.WorkflowLogs)
                .WithOne()
                .HasForeignKey(w => w.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict); // Do not cascade delete WorkflowLogs

            builder.HasMany(x => x.Assignments)
                .WithOne()
                .HasForeignKey("BuildingPlanApplicationId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Documents)
                .WithOne()
                .HasForeignKey("BuildingPlanApplicationId")
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-one relationships with workflow child entities
            builder.HasOne(x => x.Assignment)
                .WithOne()
                .HasForeignKey<BuildingPlanApplication>(x => x.AssignmentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(x => x.SiteInspection)
                .WithOne(s => s.Application)
                .HasForeignKey<BuildingPlanApplication>(x => x.SiteInspectionId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(x => x.PlanningCommitteeReview)
                .WithOne(p => p.Application)
                .HasForeignKey<BuildingPlanApplication>(x => x.PlanningCommitteeReviewId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}