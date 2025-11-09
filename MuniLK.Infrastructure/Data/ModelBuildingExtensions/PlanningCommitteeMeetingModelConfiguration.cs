using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuniLK.Domain.Entities;

namespace MuniLK.Infrastructure.Data.ModelBuildingExtensions
{
    public static class PlanningCommitteeMeetingModelConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlanningCommitteeMeetingMember>(ConfigureMember);
            modelBuilder.Entity<PlanningCommitteeMeetingApplication>(ConfigureApplication);
        }

        private static void ConfigureMember(EntityTypeBuilder<PlanningCommitteeMeetingMember> builder)
        {
            builder.HasOne(m => m.PlanningCommitteeMeeting)
                   .WithMany(p => p.Members)
                   .HasForeignKey(m => m.PlanningCommitteeMeetingId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(m => new { m.PlanningCommitteeMeetingId, m.IsDeleted });
        }

        private static void ConfigureApplication(EntityTypeBuilder<PlanningCommitteeMeetingApplication> builder)
        {
            builder.HasOne(a => a.PlanningCommitteeMeeting)
                   .WithMany(p => p.Applications)
                   .HasForeignKey(a => a.PlanningCommitteeMeetingId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(a => new { a.PlanningCommitteeMeetingId, a.IsDeleted });
        }
    }
}
