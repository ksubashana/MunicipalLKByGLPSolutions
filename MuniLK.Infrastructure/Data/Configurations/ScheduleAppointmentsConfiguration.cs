using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuniLK.Domain.Entities;

namespace MuniLK.Infrastructure.Data.Configurations
{
    public class ScheduleAppointmentsConfiguration : IEntityTypeConfiguration<ScheduleAppointments>
    {
        public void Configure(EntityTypeBuilder<ScheduleAppointments> builder)
        {
            builder.HasKey(a => a.AppointmentId);

            builder.Property(a => a.AppointmentId)
                   .ValueGeneratedOnAdd();

            builder.Property(a => a.Subject)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(a => a.Location)
                   .HasMaxLength(400);

            builder.Property(a => a.StartTime)
                   .IsRequired();

            builder.Property(a => a.EndTime)
                   .IsRequired();

            builder.Property(a => a.StartTimeZone)
                   .HasMaxLength(100);

            builder.Property(a => a.EndTimeZone)
                   .HasMaxLength(100);

            builder.Property(a => a.OwnerRole)
                   .HasMaxLength(100);

            builder.Property(a => a.CustomStyle)
                   .HasMaxLength(100);

            builder.Property(a => a.CommonGuid)
                   .HasMaxLength(100);

            builder.Property(a => a.AppTaskId)
                   .HasMaxLength(100);

            builder.Property(a => a.Guid)
                   .HasMaxLength(100);

            builder.Property(a => a.AppointmentGroup)
                   .HasMaxLength(100);

            builder.Property(a => a.CreatedBy)
                   .HasMaxLength(100);

            builder.Property(a => a.UpdatedBy)
                   .HasMaxLength(100);

            builder.Property(a => a.CreatedDate)
                   .IsRequired();

            // Configure navigation property for Owner (User)
            builder.HasOne(a => a.OwnerNav)
                   .WithMany()
                   .HasForeignKey(a => a.OwnerId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Add indexes for better query performance
            builder.HasIndex(a => a.OwnerId);
            builder.HasIndex(a => a.StartTime);
            builder.HasIndex(a => a.EndTime);
            builder.HasIndex(a => new { a.OwnerId, a.StartTime });
            builder.HasIndex(a => a.TenantId);
        }
    }
}