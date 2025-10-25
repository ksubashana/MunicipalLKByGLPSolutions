namespace MuniLK.Domain.Constants
{
    public static class Roles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string Officer = "Officer";
        public const string Citizen = "Citizen";
        public const string Inspector = "Inspector";
        public const string Auditor = "Auditor";

        // add others as needed
        public static readonly string[] SubmitBuildingPlanRoles = { SuperAdmin, Admin, Officer };
    }
}