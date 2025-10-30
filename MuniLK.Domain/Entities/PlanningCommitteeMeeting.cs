using System;
using System.Collections.Generic;
using MuniLK.Domain.Interfaces;
using MuniLK.Domain.Constants.Flows;

namespace MuniLK.Domain.Entities
{
    public class PlanningCommitteeMeeting : IHasTenant
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public string Subject { get; set; } = "Planning Committee Meeting";
        public string? Agenda { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Venue { get; set; } = string.Empty;
        public Guid ChairpersonContactId { get; set; }
        public PlanningCommitteeMeetingStatus Status { get; set; } = PlanningCommitteeMeetingStatus.Scheduled;
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public byte[]? RowVersion { get; set; }

        public ICollection<PlanningCommitteeMeetingMember> Members { get; set; } = new List<PlanningCommitteeMeetingMember>();
        public ICollection<PlanningCommitteeMeetingApplication> Applications { get; set; } = new List<PlanningCommitteeMeetingApplication>();
    }

    public class PlanningCommitteeMeetingMember : IHasTenant
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public Guid MeetingId { get; set; }
        public Guid ContactId { get; set; }
        public bool IsChair { get; set; }
        public string? Role { get; set; }
        public string? InvitationStatus { get; set; }
        public string? AttendanceStatus { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class PlanningCommitteeMeetingApplication : IHasTenant
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public Guid MeetingId { get; set; }
        public Guid BuildingPlanApplicationId { get; set; }
        public bool IsPrimaryDiscussion { get; set; } = true;
        public bool IsDeleted { get; set; }
    }
}
