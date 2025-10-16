using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuniLK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingModuleIDtoBuildingAndPlanning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "InspectionId",
                table: "SiteInspections",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "SiteInspectionId",
                table: "EntityOptionSelections",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModuleId",
                table: "buildingPlanApplications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EntityOptionSelections_SiteInspectionId",
                table: "EntityOptionSelections",
                column: "SiteInspectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntityOptionSelections_SiteInspections_SiteInspectionId",
                table: "EntityOptionSelections",
                column: "SiteInspectionId",
                principalTable: "SiteInspections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntityOptionSelections_SiteInspections_SiteInspectionId",
                table: "EntityOptionSelections");

            migrationBuilder.DropIndex(
                name: "IX_EntityOptionSelections_SiteInspectionId",
                table: "EntityOptionSelections");

            migrationBuilder.DropColumn(
                name: "SiteInspectionId",
                table: "EntityOptionSelections");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "buildingPlanApplications");

            migrationBuilder.AlterColumn<Guid>(
                name: "InspectionId",
                table: "SiteInspections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
