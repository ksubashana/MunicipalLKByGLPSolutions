using System.ComponentModel.DataAnnotations;

namespace MuniLK.Domain.Constants.Flows
{
    public enum InspectionStatus
    {
        [Display(Name = "Pending")]
        Pending,

        [Display(Name = "Approve")]
        Approve,

        [Display(Name = "Reject")]
        Reject,

        [Display(Name = "Re-Inspection Required")]
        ReInspectionRequired
    }

    public enum FinalRecommendation
    {
        [Display(Name = "Approve As Submitted")]
        ApproveAsSubmitted,

        [Display(Name = "Approve With Modifications")]
        ApproveWithModifications,

        [Display(Name = "Reject")]
        Reject,

        [Display(Name = "Re-Inspection Required")]
        ReInspectionRequired
    }

}