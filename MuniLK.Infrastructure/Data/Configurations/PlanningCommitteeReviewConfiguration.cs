using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuniLK.Domain.Entities;

namespace MuniLK.Infrastructure.Data.Configurations
{
    /// <summary>
    /// EF Core configuration for simplified PlanningCommitteeReview (review-only fields)
    /// </summary>
    public class PlanningCommitteeReviewConfiguration : IEntityTypeConfiguration<PlanningCommitteeReview>
    {
        public void Configure(EntityTypeBuilder<PlanningCommitteeReview> builder)
        {
            builder.ToTable("PlanningCommitteeReviews");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ApplicationId).IsRequired();
            builder.Property(x => x.PlanningCommitteeMeetingId).IsRequired();

            // Review inputs JSON blobs (store as NVARCHAR(max) with conservative limits if needed)
            builder.Property(x => x.InspectionReportsReviewed).HasMaxLength(2000);
            builder.Property(x => x.DocumentsReviewed).HasMaxLength(2000);
            builder.Property(x => x.ExternalAgenciesConsulted).HasMaxLength(2000);
            builder.Property(x => x.CommitteeDiscussionsSummary).HasMaxLength(4000);

            // Decision fields
            builder.Property(x => x.CommitteeDecision).HasConversion<int>().IsRequired();
            builder.Property(x => x.ConditionsImposed).HasMaxLength(4000);
            builder.Property(x => x.ReasonForRejectionOrDeferral).HasMaxLength(4000);
            builder.Property(x => x.FinalRecommendationDocumentPath).HasMaxLength(500);

            // Audit
            builder.Property(x => x.RecordedByOfficer).IsRequired().HasMaxLength(200);
            builder.Property(x => x.DigitalSignatures).HasMaxLength(2000);
            builder.Property(x => x.CreatedBy).HasMaxLength(200);
            builder.Property(x => x.ModifiedBy).HasMaxLength(200);

            // Relationships
            builder.HasOne(x => x.Application)
                   .WithOne(a => a.PlanningCommitteeReview)
                   .HasForeignKey<PlanningCommitteeReview>(x => x.ApplicationId)
                   .OnDelete(DeleteBehavior.Restrict);


        }
    }
}