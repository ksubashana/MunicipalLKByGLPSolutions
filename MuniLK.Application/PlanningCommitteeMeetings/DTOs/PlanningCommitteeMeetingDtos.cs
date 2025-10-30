using MuniLK.Domain.Constants.Flows;

namespace MuniLK.Application.PlanningCommitteeMeetings.DTOs
{
    public class PlanningCommitteeMeetingRequest
    {
        public Guid? Id { get; set; }
        public string Subject { get; set; } = "Planning Committee Meeting";
        public string? Agenda { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Venue { get; set; } = string.Empty;
        public Guid ChairpersonContactId { get; set; }
        public List<Guid> MemberContactIds { get; set; } = new();
        public List<Guid> ApplicationIds { get; set; } = new();
    }

    public class PlanningCommitteeMeetingResponse
    {
        public Guid Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string? Agenda { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Venue { get; set; } = string.Empty;
        public Guid ChairpersonContactId { get; set; }
        public PlanningCommitteeMeetingStatus Status { get; set; }
        public List<Guid> MemberContactIds { get; set; } = new();
        public List<Guid> ApplicationIds { get; set; } = new();
    }
}
