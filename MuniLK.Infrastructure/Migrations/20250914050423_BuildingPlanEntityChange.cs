using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuniLK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BuildingPlanEntityChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_buildingPlanApplications_BuildingPlanApplicationId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentLinks_buildingPlanApplications_BuildingPlanApplicationId",
                table: "DocumentLinks");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "buildingPlanApplications",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EngineerName",
                table: "buildingPlanApplications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BuildingPurpose",
                table: "buildingPlanApplications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ArchitectName",
                table: "buildingPlanApplications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedByUserId",
                table: "buildingPlanApplications",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationNumber",
                table: "buildingPlanApplications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommissionerDecision",
                table: "buildingPlanApplications",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EngineerReport",
                table: "buildingPlanApplications",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlanningReport",
                table: "buildingPlanApplications",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ScheduleAppointments",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTimeZone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EndTimeZone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AllDay = table.Column<bool>(type: "bit", nullable: false),
                    Recurrence = table.Column<bool>(type: "bit", nullable: false),
                    RecurrenceRule = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecurrenceExDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecurrenceID = table.Column<int>(type: "int", nullable: true),
                    FollowingID = table.Column<int>(type: "int", nullable: true),
                    IsBlock = table.Column<bool>(type: "bit", nullable: false),
                    IsReadOnly = table.Column<bool>(type: "bit", nullable: false),
                    Department = table.Column<int>(type: "int", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OwnerRole = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Reminder = table.Column<int>(type: "int", nullable: false),
                    CustomStyle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CommonGuid = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AppTaskId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AppointmentGroup = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleAppointments", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_ScheduleAppointments_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_WorkflowLogs_ApplicationId",
            //    table: "WorkflowLogs",
            //    column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_buildingPlanApplications_ApplicantContactId",
                table: "buildingPlanApplications",
                column: "ApplicantContactId");

            migrationBuilder.CreateIndex(
                name: "IX_buildingPlanApplications_PropertyId",
                table: "buildingPlanApplications",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleAppointments_EndTime",
                table: "ScheduleAppointments",
                column: "EndTime");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleAppointments_OwnerId",
                table: "ScheduleAppointments",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleAppointments_OwnerId_StartTime",
                table: "ScheduleAppointments",
                columns: new[] { "OwnerId", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleAppointments_StartTime",
                table: "ScheduleAppointments",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleAppointments_TenantId",
                table: "ScheduleAppointments",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_buildingPlanApplications_BuildingPlanApplicationId",
                table: "Assignments",
                column: "BuildingPlanApplicationId",
                principalTable: "buildingPlanApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_buildingPlanApplications_Contacts_ApplicantContactId",
                table: "buildingPlanApplications",
                column: "ApplicantContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_buildingPlanApplications_Properties_PropertyId",
                table: "buildingPlanApplications",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentLinks_buildingPlanApplications_BuildingPlanApplicationId",
                table: "DocumentLinks",
                column: "BuildingPlanApplicationId",
                principalTable: "buildingPlanApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowLogs_buildingPlanApplications_ApplicationId",
                table: "WorkflowLogs",
                column: "ApplicationId",
                principalTable: "buildingPlanApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_buildingPlanApplications_BuildingPlanApplicationId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_buildingPlanApplications_Contacts_ApplicantContactId",
                table: "buildingPlanApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_buildingPlanApplications_Properties_PropertyId",
                table: "buildingPlanApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentLinks_buildingPlanApplications_BuildingPlanApplicationId",
                table: "DocumentLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowLogs_buildingPlanApplications_ApplicationId",
                table: "WorkflowLogs");

            migrationBuilder.DropTable(
                name: "ScheduleAppointments");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowLogs_ApplicationId",
                table: "WorkflowLogs");

            migrationBuilder.DropIndex(
                name: "IX_buildingPlanApplications_ApplicantContactId",
                table: "buildingPlanApplications");

            migrationBuilder.DropIndex(
                name: "IX_buildingPlanApplications_PropertyId",
                table: "buildingPlanApplications");

            migrationBuilder.DropColumn(
                name: "CommissionerDecision",
                table: "buildingPlanApplications");

            migrationBuilder.DropColumn(
                name: "EngineerReport",
                table: "buildingPlanApplications");

            migrationBuilder.DropColumn(
                name: "PlanningReport",
                table: "buildingPlanApplications");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "buildingPlanApplications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EngineerName",
                table: "buildingPlanApplications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BuildingPurpose",
                table: "buildingPlanApplications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ArchitectName",
                table: "buildingPlanApplications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedByUserId",
                table: "buildingPlanApplications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationNumber",
                table: "buildingPlanApplications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_buildingPlanApplications_BuildingPlanApplicationId",
                table: "Assignments",
                column: "BuildingPlanApplicationId",
                principalTable: "buildingPlanApplications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentLinks_buildingPlanApplications_BuildingPlanApplicationId",
                table: "DocumentLinks",
                column: "BuildingPlanApplicationId",
                principalTable: "buildingPlanApplications",
                principalColumn: "Id");
        }
    }
}
