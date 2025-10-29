using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Domain.Constants.Flows
{
    public enum BuildingAndPlanSteps
    {
        [Display(Name = "Submission")]
        Submission,

        [Display(Name = "Assign Inspector")]
        AssignInspector,

        [Display(Name = "Site Inspection")]
        ToReview,

        //[Display(Name = "Planning Officer Review")]
        //PlanningOfficerReview,

        //[Display(Name = "Engineering Review")]
        //EngineeringReview,

        [Display(Name = "Planning Committee Review")]
        PlanningCommitteeReview,

        [Display(Name = "Commissioner Approval")]
        CommissionerApproval,

        [Display(Name = "Finalized")]
        Finalized,

        [Display(Name = "Rejected")]
        Rejected
    }
}
