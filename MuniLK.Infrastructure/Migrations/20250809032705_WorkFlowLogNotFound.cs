using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuniLK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WorkFlowLogNotFound : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "WorkflowLogs");

            // Create new table WorkflowLogs with all columns matching your entity
            migrationBuilder.CreateTable(
                name: "WorkflowLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionTaken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviousStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PerformedByRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedToUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsSystemGenerated = table.Column<bool>(type: "bit", nullable: false),
                    PerformedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowLogs", x => x.Id);
                    // Add foreign keys if applicable, for example:
                    // table.ForeignKey(
                    //     name: "FK_WorkflowLogs_buildingPlanApplications_ApplicationId",
                    //     column: x => x.ApplicationId,
                    //     principalTable: "buildingPlanApplications",
                    //     principalColumn: "Id",
                    //     onDelete: ReferentialAction.Cascade);
                });

            // Create index for ApplicationId for better querying performance (optional)
            migrationBuilder.CreateIndex(
                name: "IX_WorkflowLogs_ApplicationId",
                table: "WorkflowLogs",
                column: "ApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
      name: "WorkflowLog");

            // Create new table WorkflowLogs with all columns matching your entity
            migrationBuilder.CreateTable(
                name: "WorkflowLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionTaken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviousStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PerformedByRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedToUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsSystemGenerated = table.Column<bool>(type: "bit", nullable: false),
                    PerformedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowLogs", x => x.Id);
                    // Add foreign keys if applicable, for example:
                    // table.ForeignKey(
                    //     name: "FK_WorkflowLogs_buildingPlanApplications_ApplicationId",
                    //     column: x => x.ApplicationId,
                    //     principalTable: "buildingPlanApplications",
                    //     principalColumn: "Id",
                    //     onDelete: ReferentialAction.Cascade);
                });

            // Create index for ApplicationId for better querying performance (optional)
            migrationBuilder.CreateIndex(
                name: "IX_WorkflowLogs_ApplicationId",
                table: "WorkflowLogs",
                column: "ApplicationId");
        }
    }
}
