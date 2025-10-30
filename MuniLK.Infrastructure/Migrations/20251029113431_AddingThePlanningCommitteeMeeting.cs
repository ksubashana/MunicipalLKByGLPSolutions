using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuniLK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingThePlanningCommitteeMeeting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanningCommitteeMeetings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Agenda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Venue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChairpersonContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningCommitteeMeetings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanningCommitteeMeetingApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MeetingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuildingPlanApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsPrimaryDiscussion = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PlanningCommitteeMeetingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningCommitteeMeetingApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanningCommitteeMeetingApplications_PlanningCommitteeMeetings_PlanningCommitteeMeetingId",
                        column: x => x.PlanningCommitteeMeetingId,
                        principalTable: "PlanningCommitteeMeetings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlanningCommitteeMeetingMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MeetingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsChair = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvitationStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttendanceStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PlanningCommitteeMeetingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningCommitteeMeetingMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanningCommitteeMeetingMembers_PlanningCommitteeMeetings_PlanningCommitteeMeetingId",
                        column: x => x.PlanningCommitteeMeetingId,
                        principalTable: "PlanningCommitteeMeetings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanningCommitteeMeetingApplications_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingApplications",
                column: "PlanningCommitteeMeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningCommitteeMeetingMembers_PlanningCommitteeMeetingId",
                table: "PlanningCommitteeMeetingMembers",
                column: "PlanningCommitteeMeetingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanningCommitteeMeetingApplications");

            migrationBuilder.DropTable(
                name: "PlanningCommitteeMeetingMembers");

            migrationBuilder.DropTable(
                name: "PlanningCommitteeMeetings");
        }
    }
}
