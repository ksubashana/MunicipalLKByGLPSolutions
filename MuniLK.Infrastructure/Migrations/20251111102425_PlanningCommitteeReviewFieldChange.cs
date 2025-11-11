using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuniLK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PlanningCommitteeReviewFieldChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_buildingPlanApplications_PlanningCommitteeReviews_PlanningCommitteeReviewId",
                table: "buildingPlanApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanningCommitteeMeetingApplications_PlanningCommitteeMeetings_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanningCommitteeMeetingMembers_PlanningCommitteeMeetings_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingMembers");

            migrationBuilder.DropIndex(
                name: "IX_PlanningCommitteeMeetingMembers_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingMembers");

            migrationBuilder.DropIndex(
                name: "IX_PlanningCommitteeMeetingApplications_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingApplications");

            migrationBuilder.DropIndex(
                name: "IX_buildingPlanApplications_PlanningCommitteeReviewId",
                table: "buildingPlanApplications");

            migrationBuilder.DropColumn(
                name: "ChairpersonName",
                table: "PlanningCommitteeReviews");

            migrationBuilder.DropColumn(
                name: "CommitteeType",
                table: "PlanningCommitteeReviews");

            migrationBuilder.DropColumn(
                name: "MeetingDate",
                table: "PlanningCommitteeReviews");

            migrationBuilder.DropColumn(
                name: "MeetingReferenceNo",
                table: "PlanningCommitteeReviews");

            migrationBuilder.DropColumn(
                name: "MembersPresent",
                table: "PlanningCommitteeReviews");

            migrationBuilder.DropColumn(
                name: "MeetingId",
                table: "PlanningCommitteeMeetingMembers");

            migrationBuilder.DropColumn(
                name: "MeetingId",
                table: "PlanningCommitteeMeetingApplications");

            migrationBuilder.AddColumn<Guid>(
                name: "PlanningCommitteeMeetingId",
                table: "PlanningCommitteeReviews",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingMembers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingApplications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanningCommitteeReviews_ApplicationId",
                table: "PlanningCommitteeReviews",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanningCommitteeReviews_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeReviews",
                column: "PlanningCommitteeMeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningCommitteeMeetingMembers_PlanningCommitteeMeetingId_IsDeleted",
                table: "PlanningCommitteeMeetingMembers",
                columns: new[] { "PlanningCommitteeMeetingId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_PlanningCommitteeMeetingApplications_PlanningCommitteeMeetingId_IsDeleted",
                table: "PlanningCommitteeMeetingApplications",
                columns: new[] { "PlanningCommitteeMeetingId", "IsDeleted" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningCommitteeMeetingApplications_PlanningCommitteeMeetings_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingApplications",
                column: "PlanningCommitteeMeetingId",
                principalTable: "PlanningCommitteeMeetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningCommitteeMeetingMembers_PlanningCommitteeMeetings_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingMembers",
                column: "PlanningCommitteeMeetingId",
                principalTable: "PlanningCommitteeMeetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningCommitteeReviews_PlanningCommitteeMeetings_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeReviews",
                column: "PlanningCommitteeMeetingId",
                principalTable: "PlanningCommitteeMeetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningCommitteeReviews_buildingPlanApplications_ApplicationId",
                table: "PlanningCommitteeReviews",
                column: "ApplicationId",
                principalTable: "buildingPlanApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanningCommitteeMeetingApplications_PlanningCommitteeMeetings_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanningCommitteeMeetingMembers_PlanningCommitteeMeetings_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanningCommitteeReviews_PlanningCommitteeMeetings_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanningCommitteeReviews_buildingPlanApplications_ApplicationId",
                table: "PlanningCommitteeReviews");

            migrationBuilder.DropIndex(
                name: "IX_PlanningCommitteeReviews_ApplicationId",
                table: "PlanningCommitteeReviews");

            migrationBuilder.DropIndex(
                name: "IX_PlanningCommitteeReviews_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeReviews");

            migrationBuilder.DropIndex(
                name: "IX_PlanningCommitteeMeetingMembers_PlanningCommitteeMeetingId_IsDeleted",
                table: "PlanningCommitteeMeetingMembers");

            migrationBuilder.DropIndex(
                name: "IX_PlanningCommitteeMeetingApplications_PlanningCommitteeMeetingId_IsDeleted",
                table: "PlanningCommitteeMeetingApplications");

            migrationBuilder.DropColumn(
                name: "PlanningCommitteeMeetingId",
                table: "PlanningCommitteeReviews");

            migrationBuilder.AddColumn<string>(
                name: "ChairpersonName",
                table: "PlanningCommitteeReviews",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CommitteeType",
                table: "PlanningCommitteeReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "MeetingDate",
                table: "PlanningCommitteeReviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MeetingReferenceNo",
                table: "PlanningCommitteeReviews",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MembersPresent",
                table: "PlanningCommitteeReviews",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingMembers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "MeetingId",
                table: "PlanningCommitteeMeetingMembers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingApplications",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "MeetingId",
                table: "PlanningCommitteeMeetingApplications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PlanningCommitteeMeetingMembers_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingMembers",
                column: "PlanningCommitteeMeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningCommitteeMeetingApplications_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingApplications",
                column: "PlanningCommitteeMeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_buildingPlanApplications_PlanningCommitteeReviewId",
                table: "buildingPlanApplications",
                column: "PlanningCommitteeReviewId",
                unique: true,
                filter: "[PlanningCommitteeReviewId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_buildingPlanApplications_PlanningCommitteeReviews_PlanningCommitteeReviewId",
                table: "buildingPlanApplications",
                column: "PlanningCommitteeReviewId",
                principalTable: "PlanningCommitteeReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningCommitteeMeetingApplications_PlanningCommitteeMeetings_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingApplications",
                column: "PlanningCommitteeMeetingId",
                principalTable: "PlanningCommitteeMeetings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningCommitteeMeetingMembers_PlanningCommitteeMeetings_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingMembers",
                column: "PlanningCommitteeMeetingId",
                principalTable: "PlanningCommitteeMeetings",
                principalColumn: "Id");
        }
    }
}
