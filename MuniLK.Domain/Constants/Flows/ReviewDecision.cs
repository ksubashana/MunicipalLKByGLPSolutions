using System.ComponentModel.DataAnnotations;

namespace MuniLK.Domain.Constants.Flows
{
    /// <summary>
    /// Represents the possible decisions for each review step in the building plan workflow
    /// </summary>
    public enum ReviewDecision
    {
        [Display(Name = "Pending")]
        Pending = 0,

        [Display(Name = "Approved")]
        Approved = 1,

        [Display(Name = "Rejected")]
        Rejected = 2,

        [Display(Name = "Clarification Required")]
        ClarificationRequired = 3
    }
}