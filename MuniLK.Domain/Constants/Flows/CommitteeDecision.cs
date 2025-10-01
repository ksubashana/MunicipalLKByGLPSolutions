using System.ComponentModel.DataAnnotations;

namespace MuniLK.Domain.Constants.Flows
{
    /// <summary>
    /// Represents the possible decisions for Planning Committee Review
    /// </summary>
    public enum CommitteeDecision
    {
        [Display(Name = "Pending")]
        Pending = 0,

        [Display(Name = "Approve")]
        Approve = 1,

        [Display(Name = "Approve with Conditions")]
        ApproveWithConditions = 2,

        [Display(Name = "Reject")]
        Reject = 3,

        [Display(Name = "Defer for Clarifications")]
        DeferForClarifications = 4
    }
}