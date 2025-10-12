using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuniLK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkflowForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanningCommitteeReviews_buildingPlanApplications_ApplicationId",
                table: "PlanningCommitteeReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_SiteInspections_buildingPlanApplications_ApplicationId",
                table: "SiteInspections");

            migrationBuilder.DropIndex(
                name: "IX_SiteInspections_ApplicationId",
                table: "SiteInspections");

            migrationBuilder.DropIndex(
                name: "IX_PlanningCommitteeReviews_ApplicationId",
                table: "PlanningCommitteeReviews");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignmentId",
                table: "buildingPlanApplications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PlanningCommitteeReviewId",
                table: "buildingPlanApplications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SiteInspectionId",
                table: "buildingPlanApplications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_buildingPlanApplications_AssignmentId",
                table: "buildingPlanApplications",
                column: "AssignmentId",
                unique: true,
                filter: "[AssignmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_buildingPlanApplications_PlanningCommitteeReviewId",
                table: "buildingPlanApplications",
                column: "PlanningCommitteeReviewId",
                unique: true,
                filter: "[PlanningCommitteeReviewId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_buildingPlanApplications_SiteInspectionId",
                table: "buildingPlanApplications",
                column: "SiteInspectionId",
                unique: true,
                filter: "[SiteInspectionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_buildingPlanApplications_Assignments_AssignmentId",
                table: "buildingPlanApplications",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_buildingPlanApplications_PlanningCommitteeReviews_PlanningCommitteeReviewId",
                table: "buildingPlanApplications",
                column: "PlanningCommitteeReviewId",
                principalTable: "PlanningCommitteeReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_buildingPlanApplications_SiteInspections_SiteInspectionId",
                table: "buildingPlanApplications",
                column: "SiteInspectionId",
                principalTable: "SiteInspections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_buildingPlanApplications_Assignments_AssignmentId",
                table: "buildingPlanApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_buildingPlanApplications_PlanningCommitteeReviews_PlanningCommitteeReviewId",
                table: "buildingPlanApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_buildingPlanApplications_SiteInspections_SiteInspectionId",
                table: "buildingPlanApplications");

            migrationBuilder.DropIndex(
                name: "IX_buildingPlanApplications_AssignmentId",
                table: "buildingPlanApplications");

            migrationBuilder.DropIndex(
                name: "IX_buildingPlanApplications_PlanningCommitteeReviewId",
                table: "buildingPlanApplications");

            migrationBuilder.DropIndex(
                name: "IX_buildingPlanApplications_SiteInspectionId",
                table: "buildingPlanApplications");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "buildingPlanApplications");

            migrationBuilder.DropColumn(
                name: "PlanningCommitteeReviewId",
                table: "buildingPlanApplications");

            migrationBuilder.DropColumn(
                name: "SiteInspectionId",
                table: "buildingPlanApplications");

            migrationBuilder.CreateIndex(
                name: "IX_SiteInspections_ApplicationId",
                table: "SiteInspections",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningCommitteeReviews_ApplicationId",
                table: "PlanningCommitteeReviews",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningCommitteeReviews_buildingPlanApplications_ApplicationId",
                table: "PlanningCommitteeReviews",
                column: "ApplicationId",
                principalTable: "buildingPlanApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SiteInspections_buildingPlanApplications_ApplicationId",
                table: "SiteInspections",
                column: "ApplicationId",
                principalTable: "buildingPlanApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
