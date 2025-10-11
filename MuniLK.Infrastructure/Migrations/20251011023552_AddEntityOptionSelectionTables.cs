using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuniLK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityOptionSelectionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OptionGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OptionItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionItems_OptionGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "OptionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityOptionSelections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OptionItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityOptionSelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityOptionSelections_OptionItems_OptionItemId",
                        column: x => x.OptionItemId,
                        principalTable: "OptionItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityOptionSelections_EntityId_EntityType_ModuleId",
                table: "EntityOptionSelections",
                columns: new[] { "EntityId", "EntityType", "ModuleId" });

            migrationBuilder.CreateIndex(
                name: "IX_EntityOptionSelections_OptionItemId",
                table: "EntityOptionSelections",
                column: "OptionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionItems_GroupId",
                table: "OptionItems",
                column: "GroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityOptionSelections");

            migrationBuilder.DropTable(
                name: "OptionItems");

            migrationBuilder.DropTable(
                name: "OptionGroups");
        }
    }
}
