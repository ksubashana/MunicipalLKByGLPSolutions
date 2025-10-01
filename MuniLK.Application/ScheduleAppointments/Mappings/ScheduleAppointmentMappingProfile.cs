using MuniLK.Application.ScheduleAppointment.DTOs;
using MuniLK.Domain.Entities;

namespace MuniLK.Application.ScheduleAppointment.Mappings
{
    public static class ScheduleAppointmentMappingProfile
    {
        public static ScheduleAppointments ToEntity(this ScheduleAppointmentRequest dto, Guid? tenantId, string? createdBy)
        {
            return new ScheduleAppointments
            {
                TenantId = tenantId,
                Subject = dto.Subject,
                Location = dto.Location,
                Description = dto.Description,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                StartTimeZone = dto.StartTimeZone,
                EndTimeZone = dto.EndTimeZone,
                AllDay = dto.AllDay,
                Recurrence = dto.Recurrence,
                RecurrenceRule = dto.RecurrenceRule,
                RecurrenceExDate = dto.RecurrenceExDate,
                RecurrenceID = dto.RecurrenceID,
                FollowingID = dto.FollowingID,
                IsBlock = dto.IsBlock,
                IsReadOnly = dto.IsReadOnly,
                Department = dto.Department,
                OwnerId = dto.OwnerId,
                OwnerRole = dto.OwnerRole,
                Priority = dto.Priority,
                Reminder = dto.Reminder,
                CustomStyle = dto.CustomStyle,
                AppointmentGroup = dto.AppointmentGroup,
                CreatedBy = createdBy ?? string.Empty,
                CreatedDate = DateTime.UtcNow
            };
        }

        public static ScheduleAppointmentResponse ToResponse(this ScheduleAppointments entity)
        {
            return new ScheduleAppointmentResponse
            {
                AppointmentId = entity.AppointmentId,
                Subject = entity.Subject,
                Location = entity.Location,
                Description = entity.Description,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                StartTimeZone = entity.StartTimeZone,
                EndTimeZone = entity.EndTimeZone,
                AllDay = entity.AllDay,
                Recurrence = entity.Recurrence,
                RecurrenceRule = entity.RecurrenceRule,
                RecurrenceExDate = entity.RecurrenceExDate,
                RecurrenceID = entity.RecurrenceID,
                FollowingID = entity.FollowingID,
                IsBlock = entity.IsBlock,
                IsReadOnly = entity.IsReadOnly,
                Department = entity.Department,
                OwnerId = entity.OwnerId,
                OwnerName = entity.OwnerNav?.Username ?? string.Empty,
                OwnerRole = entity.OwnerRole,
                Priority = entity.Priority,
                Reminder = entity.Reminder,
                CustomStyle = entity.CustomStyle,
                AppointmentGroup = entity.AppointmentGroup,
                CreatedBy = entity.CreatedBy,
                CreatedDate = entity.CreatedDate,
                UpdatedBy = entity.UpdatedBy ?? string.Empty,
                UpdatedDate = entity.UpdatedDate,
                
                // Syncfusion-specific mappings
                Id = entity.AppointmentId.ToString(),
                StartTimezone = entity.StartTimeZone,
                EndTimezone = entity.EndTimeZone,
                IsAllDay = entity.AllDay
            };
        }

        public static void UpdateFromDto(this ScheduleAppointments entity, UpdateScheduleAppointmentRequest dto, string? updatedBy)
        {
            entity.Subject = dto.Subject;
            entity.Location = dto.Location;
            entity.Description = dto.Description;
            entity.StartTime = dto.StartTime;
            entity.EndTime = dto.EndTime;
            entity.StartTimeZone = dto.StartTimeZone;
            entity.EndTimeZone = dto.EndTimeZone;
            entity.AllDay = dto.AllDay;
            entity.Recurrence = dto.Recurrence;
            entity.RecurrenceRule = dto.RecurrenceRule;
            entity.RecurrenceExDate = dto.RecurrenceExDate;
            entity.RecurrenceID = dto.RecurrenceID;
            entity.FollowingID = dto.FollowingID;
            entity.IsBlock = dto.IsBlock;
            entity.IsReadOnly = dto.IsReadOnly;
            entity.Department = dto.Department;
            entity.OwnerId = dto.OwnerId;
            entity.OwnerRole = dto.OwnerRole;
            entity.Priority = dto.Priority;
            entity.Reminder = dto.Reminder;
            entity.CustomStyle = dto.CustomStyle;
            entity.AppointmentGroup = dto.AppointmentGroup;
            entity.UpdatedBy = updatedBy ?? string.Empty;
            entity.UpdatedDate = DateTime.UtcNow;
        }
    }
}