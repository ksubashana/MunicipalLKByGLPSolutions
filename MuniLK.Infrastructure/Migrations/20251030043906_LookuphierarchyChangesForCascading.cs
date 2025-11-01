using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuniLK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LookuphierarchyChangesForCascading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentLookupId",
                table: "Lookups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lookups_ParentLookupId",
                table: "Lookups",
                column: "ParentLookupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lookups_Lookups_ParentLookupId",
                table: "Lookups",
                column: "ParentLookupId",
                principalTable: "Lookups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lookups_Lookups_ParentLookupId",
                table: "Lookups");

            migrationBuilder.DropIndex(
                name: "IX_Lookups_ParentLookupId",
                table: "Lookups");

            migrationBuilder.DropColumn(
                name: "ParentLookupId",
                table: "Lookups");
        }
    }
}
