using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuniLK.Domain.Interfaces;

namespace MuniLK.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ScheduleAppointments : IHasTenant
    {
        [Key]
        public int AppointmentId { get; set; }

        // Tenant support for multi-tenancy
        public Guid? TenantId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Subject { get; set; } = string.Empty;

        [MaxLength(400)]
        public string Location { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartTime { get; set; } = DateTime.Now;

        [Required]
        public DateTime EndTime { get; set; } = DateTime.Now;

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

        // Foreign Key to Department (nullable for flexibility)
        public int? Department { get; set; }

        // Foreign Key to User/Owner - Updated to support Guid-based Users
        [ForeignKey("OwnerNav")]
        public Guid? OwnerId { get; set; }

        // User role for display purposes
        [MaxLength(100)]
        public string OwnerRole { get; set; } = string.Empty;

        public int Priority { get; set; } = 0;

        public int Reminder { get; set; } = 0;

        [MaxLength(100)]
        public string CustomStyle { get; set; } = string.Empty;

        [MaxLength(100)]
        public string CommonGuid { get; set; } = string.Empty;

        [MaxLength(100)]
        public string AppTaskId { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Guid { get; set; } = string.Empty;

        [MaxLength(100)]
        public string AppointmentGroup { get; set; } = string.Empty;

        // Audit Fields - Using consistent naming
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string UpdatedBy { get; set; } = string.Empty;

        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        public virtual User? OwnerNav { get; set; }

        // Department navigation (if needed later)
        //public virtual Department? DepartmentNav { get; set; }
    }

}
