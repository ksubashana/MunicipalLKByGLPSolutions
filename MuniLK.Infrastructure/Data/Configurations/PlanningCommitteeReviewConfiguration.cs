using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuniLK.Domain.Entities;

namespace MuniLK.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for PlanningCommitteeReview
    /// </summary>
    public class PlanningCommitteeReviewConfiguration : IEntityTypeConfiguration<PlanningCommitteeReview>
    {
        public void Configure(EntityTypeBuilder<PlanningCommitteeReview> builder)
        {
            builder.ToTable("PlanningCommitteeReviews");
            
            builder.HasKey(x => x.Id);

            // Configure required properties
            builder.Property(x => x.ApplicationId)
                .IsRequired();

            builder.Property(x => x.MeetingDate)
                .IsRequired();

            builder.Property(x => x.CommitteeType)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.MeetingReferenceNo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.ChairpersonName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.MembersPresent)
                .IsRequired()
                .HasMaxLength(4000); // JSON string

            // Review inputs
            builder.Property(x => x.InspectionReportsReviewed)
                .HasMaxLength(2000); // JSON string

            builder.Property(x => x.DocumentsReviewed)
                .HasMaxLength(2000); // JSON string

            builder.Property(x => x.ExternalAgenciesConsulted)
                .HasMaxLength(2000); // JSON string

            builder.Property(x => x.CommitteeDiscussionsSummary)
                .HasMaxLength(4000);

            // Decision & Recommendations
            builder.Property(x => x.CommitteeDecision)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.ConditionsImposed)
                .HasMaxLength(4000);

            builder.Property(x => x.ReasonForRejectionOrDeferral)
                .HasMaxLength(4000);

            builder.Property(x => x.FinalRecommendationDocumentPath)
                .HasMaxLength(500);

            // Audit & Sign-Off
            builder.Property(x => x.RecordedByOfficer)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.DigitalSignatures)
                .HasMaxLength(2000); // JSON string

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(200);

            builder.Property(x => x.ModifiedBy)
                .HasMaxLength(200);

            // Configure relationship with BuildingPlanApplication
            builder.HasOne(x => x.Application)
                .WithMany()
                .HasForeignKey(x => x.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict); // Don't cascade delete
        }
    }
}