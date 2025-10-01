using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuniLK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SiteInspectionEnhancements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteInspections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OfficersPresent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GpsCoordinates = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotosPaths = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessRoadWidthCondition = table.Column<bool>(type: "bit", nullable: true),
                    AccessRoadWidthNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoundaryVerification = table.Column<bool>(type: "bit", nullable: true),
                    BoundaryVerificationNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Topography = table.Column<bool>(type: "bit", nullable: true),
                    TopographyNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExistingStructures = table.Column<bool>(type: "bit", nullable: true),
                    ExistingStructuresNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EncroachmentsReservations = table.Column<bool>(type: "bit", nullable: true),
                    EncroachmentsReservationsNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchesSurveyPlan = table.Column<bool>(type: "bit", nullable: true),
                    MatchesSurveyPlanNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZoningCompatible = table.Column<bool>(type: "bit", nullable: true),
                    ZoningCompatibleNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetbacksObserved = table.Column<bool>(type: "bit", nullable: true),
                    SetbacksObservedNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrontSetback = table.Column<bool>(type: "bit", nullable: true),
                    RearSetback = table.Column<bool>(type: "bit", nullable: true),
                    SideSetbacks = table.Column<bool>(type: "bit", nullable: true),
                    EnvironmentalConcerns = table.Column<bool>(type: "bit", nullable: true),
                    EnvironmentalConcernsNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredModifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClearancesRequired = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalRecommendation = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteInspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteInspections_buildingPlanApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "buildingPlanApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiteInspections_ApplicationId",
                table: "SiteInspections",
                column: "ApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteInspections");
        }
    }
}
