using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuniLK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNavigationColumnnamesPlanningMeeting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "MeetingId",
                table: "PlanningCommitteeMeetingMembers");

            migrationBuilder.DropColumn(
                name: "MeetingId",
                table: "PlanningCommitteeMeetingApplications");

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

            migrationBuilder.DropIndex(
                name: "IX_PlanningCommitteeMeetingMembers_PlanningCommitteeMeetingId_IsDeleted",
                table: "PlanningCommitteeMeetingMembers");

            migrationBuilder.DropIndex(
                name: "IX_PlanningCommitteeMeetingApplications_PlanningCommitteeMeetingId_IsDeleted",
                table: "PlanningCommitteeMeetingApplications");

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
