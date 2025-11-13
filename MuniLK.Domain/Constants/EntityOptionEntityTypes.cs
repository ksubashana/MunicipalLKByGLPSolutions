namespace MuniLK.Domain.Constants
{
    /// <summary>
    /// Centralized constants for EntityType discriminator values used in EntityOptionSelection rows
    /// to persist multi-select / checkbox lookup associations.
    /// Keep existing string values unchanged to avoid data migration issues.
    /// </summary>
    public static class EntityOptionEntityTypes
    {
        // Site Inspection related selections
        public const string SiteInspection = "SiteInspection";

        // Planning Committee (generic catch-all if needed)
        public const string PlanningCommittee = "PlanningCommittee";

        // Planning Committee Review specific grouped selections
        public const string PlanningCommitteeInspectionReports = "PCReview_InspectionReports";
        public const string PlanningCommitteeDocumentsReviewed = "PCReview_DocumentsReviewed";
        public const string PlanningCommitteeExternalAgencies = "PCReview_ExternalAgencies";
    }
}