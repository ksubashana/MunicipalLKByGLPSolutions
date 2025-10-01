using System.ComponentModel.DataAnnotations;

namespace MuniLK.Application.ScheduleAppointment.DTOs
{
    public class ScheduleAppointmentRequest
    {
        [Required]
        [MaxLength(200)]
        public string Subject { get; set; } = string.Empty;

        [MaxLength(400)]
        public string Location { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [MaxLength(100)]
        public string StartTimeZone { get; set; } = string.Empty;

        [MaxLength(100)]
        public string EndTimeZone { get; set; } = string.Empty;

        public bool AllDay { get; set; } = false;

        public bool Recurrence { get; set; } = false;

        public string RecurrenceRule { get; set; } = string.Empty;

        public string RecurrenceExDate { get; set; } = string.Empty;

        public int? RecurrenceID { get; set; }

        public int? FollowingID { get; set; }

        public bool IsBlock { get; set; } = false;

        public bool IsReadOnly { get; set; } = false;

        public int? Department { get; set; }

        public Guid? OwnerId { get; set; }

        public string OwnerRole { get; set; } = string.Empty;

        public int Priority { get; set; } = 0;

        public int Reminder { get; set; } = 0;

        [MaxLength(100)]
        public string CustomStyle { get; set; } = string.Empty;

        [MaxLength(100)]
        public string AppointmentGroup { get; set; } = string.Empty;
    }
}