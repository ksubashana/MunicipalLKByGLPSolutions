using System.ComponentModel.DataAnnotations;

namespace MuniLK.Domain.Constants.Flows
{
    /// <summary>
    /// Represents the types of planning committees that can review building plans
    /// </summary>
    public enum CommitteeType
    {
        [Display(Name = "Planning Committee")]
        PlanningCommittee = 1,

        [Display(Name = "Sub Committee")]
        SubCommittee = 2,

        [Display(Name = "Main Committee")]
        MainCommittee = 3
    }
}