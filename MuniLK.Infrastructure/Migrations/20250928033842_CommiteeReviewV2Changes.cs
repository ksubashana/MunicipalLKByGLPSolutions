using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuniLK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CommiteeReviewV2Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanningCommitteeReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeetingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommitteeType = table.Column<int>(type: "int", nullable: false),
                    MeetingReferenceNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChairpersonName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MembersPresent = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    InspectionReportsReviewed = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DocumentsReviewed = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ApplicantRepresented = table.Column<bool>(type: "bit", nullable: false),
                    ExternalAgenciesConsulted = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CommitteeDiscussionsSummary = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CommitteeDecision = table.Column<int>(type: "int", nullable: false),
                    ConditionsImposed = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ReasonForRejectionOrDeferral = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    FinalRecommendationDocumentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RecordedByOfficer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ApprovalTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DigitalSignatures = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningCommitteeReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanningCommitteeReviews_buildingPlanApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "buildingPlanApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanningCommitteeReviews_ApplicationId",
                table: "PlanningCommitteeReviews",
                column: "ApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanningCommitteeReviews");
        }
    }
}
