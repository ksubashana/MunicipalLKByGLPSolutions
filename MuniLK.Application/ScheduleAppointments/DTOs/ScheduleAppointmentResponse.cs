namespace MuniLK.Application.ScheduleAppointment.DTOs
{
    public class ScheduleAppointmentResponse
    {
        public int AppointmentId { get; set; }

        public string Subject { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string StartTimeZone { get; set; } = string.Empty;

        public string EndTimeZone { get; set; } = string.Empty;

        public bool AllDay { get; set; }

        public bool Recurrence { get; set; }

        public string RecurrenceRule { get; set; } = string.Empty;

        public string RecurrenceExDate { get; set; } = string.Empty;

        public int? RecurrenceID { get; set; }

        public int? FollowingID { get; set; }

        public bool IsBlock { get; set; }

        public bool IsReadOnly { get; set; }

        public int? Department { get; set; }

        public Guid? OwnerId { get; set; }

        public string OwnerName { get; set; } = string.Empty;

        public string OwnerRole { get; set; } = string.Empty;

        public int Priority { get; set; }

        public int Reminder { get; set; }

        public string CustomStyle { get; set; } = string.Empty;

        public string AppointmentGroup { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;

        public DateTime? UpdatedDate { get; set; }

        // Additional properties for UI
        public string Id { get; set; } = string.Empty; // For Syncfusion mapping

        public string StartTimezone { get; set; } = string.Empty; // Syncfusion compatibility

        public string EndTimezone { get; set; } = string.Empty; // Syncfusion compatibility

        public bool IsAllDay { get; set; } // Syncfusion compatibility
    }
}