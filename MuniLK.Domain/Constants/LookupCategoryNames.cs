using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Domain.Constants
{
    public enum LookupCategoryNames
    {
        LandExtentUnit,
        NewCategoryName,
        PropertyTypes,
        PropertyOwnershipType,
        ElectoralDivision,
        OwnerAssociationType,
        ConstructionType,
        RoadAccessType,
        GSDivision,
        Zone,
        DocumentType,
        Modules,
        ClearanceTypes,
        Districts,
        Provinces,
        // Added for Planning Committee Review lookups
        InspectionReports,           // e.g. site_inspection_1 etc.
        PlanningReviewDocuments     // documents reviewed in committee
    }
}
