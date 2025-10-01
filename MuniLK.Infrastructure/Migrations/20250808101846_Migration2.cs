using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuniLK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyOwner_Contacts_ContactId",
                table: "PropertyOwner");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyOwner_Properties_PropertyId",
                table: "PropertyOwner");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyOwner",
                table: "PropertyOwner");

            migrationBuilder.RenameTable(
                name: "PropertyOwner",
                newName: "PropertyOwners");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyOwner_ContactId",
                table: "PropertyOwners",
                newName: "IX_PropertyOwners_ContactId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyOwners",
                table: "PropertyOwners",
                columns: new[] { "PropertyId", "ContactId" });

            migrationBuilder.CreateTable(
                name: "DocumentLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LinkContext = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LinkedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LinkedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuildingPlanApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentLinks_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentLinks_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentLinks_buildingPlanApplications_BuildingPlanApplicationId",
                        column: x => x.BuildingPlanApplicationId,
                        principalTable: "buildingPlanApplications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentLinks_BuildingPlanApplicationId",
                table: "DocumentLinks",
                column: "BuildingPlanApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentLinks_DocumentId",
                table: "DocumentLinks",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentLinks_ModuleId",
                table: "DocumentLinks",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyOwners_Contacts_ContactId",
                table: "PropertyOwners",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyOwners_Properties_PropertyId",
                table: "PropertyOwners",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyOwners_Contacts_ContactId",
                table: "PropertyOwners");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyOwners_Properties_PropertyId",
                table: "PropertyOwners");

            migrationBuilder.DropTable(
                name: "DocumentLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyOwners",
                table: "PropertyOwners");

            migrationBuilder.RenameTable(
                name: "PropertyOwners",
                newName: "PropertyOwner");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyOwners_ContactId",
                table: "PropertyOwner",
                newName: "IX_PropertyOwner_ContactId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyOwner",
                table: "PropertyOwner",
                columns: new[] { "PropertyId", "ContactId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyOwner_Contacts_ContactId",
                table: "PropertyOwner",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyOwner_Properties_PropertyId",
                table: "PropertyOwner",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
