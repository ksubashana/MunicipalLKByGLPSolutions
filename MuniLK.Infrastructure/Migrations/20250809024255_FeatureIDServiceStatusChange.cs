using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuniLK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FeatureIDServiceStatusChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_buildingPlanApplications_Lookups_StatusId",
                table: "buildingPlanApplications");

            migrationBuilder.DropIndex(
                name: "IX_buildingPlanApplications_StatusId",
                table: "buildingPlanApplications");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "buildingPlanApplications");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "buildingPlanApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "buildingPlanApplications");

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "buildingPlanApplications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_buildingPlanApplications_StatusId",
                table: "buildingPlanApplications",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_buildingPlanApplications_Lookups_StatusId",
                table: "buildingPlanApplications",
                column: "StatusId",
                principalTable: "Lookups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
