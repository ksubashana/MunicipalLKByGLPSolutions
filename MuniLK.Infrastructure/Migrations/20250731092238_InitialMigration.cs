using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuniLK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConfigKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConfigJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NIC = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Licenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ProfessionalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profession = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NICOrBRN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Municipality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogEntries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceContext = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RequestId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RequestPath = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    MachineName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ThreadId = table.Column<int>(type: "int", nullable: true),
                    Protocol = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Method = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Host = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConnectionId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TraceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpanId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LookupCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconCssClass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsCoreModule = table.Column<bool>(type: "bit", nullable: false),
                    ParentModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modules_Modules_ParentModuleId",
                        column: x => x.ParentModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Subdomain = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantId);
                });

            migrationBuilder.CreateTable(
                name: "Lookups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LookupCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lookups_LookupCategories_LookupCategoryId",
                        column: x => x.LookupCategoryId,
                        principalTable: "LookupCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlobPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "buildingPlanApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuildingPurpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoOfFloors = table.Column<int>(type: "int", nullable: false),
                    ArchitectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngineerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplicantContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmittedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_buildingPlanApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_buildingPlanApplications_Lookups_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Lookups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlobPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DocumentTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UploadedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Lookups_DocumentStatusId",
                        column: x => x.DocumentStatusId,
                        principalTable: "Lookups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Lookups_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "Lookups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PropertyId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PropertyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssessmentValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WardNumber = table.Column<int>(type: "int", nullable: true),
                    ZoneId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LandExtentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LandAreaInSqMeters = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TitleDeedNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OwnershipTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsCommercialUse = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfBuildings = table.Column<int>(type: "int", nullable: true),
                    ConstructionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoadAccessTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GSDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ElectoralDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WaterConnection = table.Column<bool>(type: "bit", nullable: true),
                    ElectricityConnection = table.Column<bool>(type: "bit", nullable: true),
                    LastAssessmentYear = table.Column<int>(type: "int", nullable: true),
                    IsDisputed = table.Column<bool>(type: "bit", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LocationCoordinates = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_Lookups_ConstructionTypeId",
                        column: x => x.ConstructionTypeId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Properties_Lookups_ElectoralDivisionId",
                        column: x => x.ElectoralDivisionId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Properties_Lookups_GSDivisionId",
                        column: x => x.GSDivisionId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Properties_Lookups_LandExtentId",
                        column: x => x.LandExtentId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Properties_Lookups_OwnershipTypeId",
                        column: x => x.OwnershipTypeId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Properties_Lookups_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Properties_Lookups_RoadAccessTypeId",
                        column: x => x.RoadAccessTypeId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Properties_Lookups_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Inspection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduledOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InspectorUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Report = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BuildingPlanApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspection_buildingPlanApplications_BuildingPlanApplicationId",
                        column: x => x.BuildingPlanApplicationId,
                        principalTable: "buildingPlanApplications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkflowLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionTaken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviousStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PerformedByRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedToUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSystemGenerated = table.Column<bool>(type: "bit", nullable: false),
                    PerformedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BuildingPlanApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowLog_buildingPlanApplications_BuildingPlanApplicationId",
                        column: x => x.BuildingPlanApplicationId,
                        principalTable: "buildingPlanApplications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "buildingPlanApplicationDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuildingPlanApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LinkContext = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LinkedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LinkedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_buildingPlanApplicationDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_buildingPlanApplicationDocuments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_buildingPlanApplicationDocuments_buildingPlanApplications_BuildingPlanApplicationId",
                        column: x => x.BuildingPlanApplicationId,
                        principalTable: "buildingPlanApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "licenseDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LicenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LinkContext = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LinkedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LinkedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_licenseDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_licenseDocuments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_licenseDocuments_Licenses_LicenseId",
                        column: x => x.LicenseId,
                        principalTable: "Licenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyOwner",
                columns: table => new
                {
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnershipType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyOwner", x => new { x.PropertyId, x.ContactId });
                    table.ForeignKey(
                        name: "FK_PropertyOwner_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertyOwner_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_buildingPlanApplicationDocuments_BuildingPlanApplicationId",
                table: "buildingPlanApplicationDocuments",
                column: "BuildingPlanApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_buildingPlanApplicationDocuments_DocumentId",
                table: "buildingPlanApplicationDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_buildingPlanApplications_StatusId",
                table: "buildingPlanApplications",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientConfigurations_TenantId_ConfigKey",
                table: "ClientConfigurations",
                columns: new[] { "TenantId", "ConfigKey" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Email",
                table: "Contacts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_NIC",
                table: "Contacts",
                column: "NIC",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_PhoneNumber",
                table: "Contacts",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentStatusId",
                table: "Documents",
                column: "DocumentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentTypeId",
                table: "Documents",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_BuildingPlanApplicationId",
                table: "Inspection",
                column: "BuildingPlanApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_licenseDocuments_DocumentId",
                table: "licenseDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_licenseDocuments_LicenseId",
                table: "licenseDocuments",
                column: "LicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_LookupCategories_Name_TenantId",
                table: "LookupCategories",
                columns: new[] { "Name", "TenantId" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Lookups_LookupCategoryId_Value_TenantId",
                table: "Lookups",
                columns: new[] { "LookupCategoryId", "Value", "TenantId" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_Code",
                table: "Modules",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_ParentModuleId",
                table: "Modules",
                column: "ParentModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_ConstructionTypeId",
                table: "Properties",
                column: "ConstructionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_ElectoralDivisionId",
                table: "Properties",
                column: "ElectoralDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_GSDivisionId",
                table: "Properties",
                column: "GSDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_LandExtentId",
                table: "Properties",
                column: "LandExtentId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_OwnershipTypeId",
                table: "Properties",
                column: "OwnershipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyTypeId",
                table: "Properties",
                column: "PropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_RoadAccessTypeId",
                table: "Properties",
                column: "RoadAccessTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_ZoneId",
                table: "Properties",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyOwner_ContactId",
                table: "PropertyOwner",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ModuleId",
                table: "Reports",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowLog_BuildingPlanApplicationId",
                table: "WorkflowLog",
                column: "BuildingPlanApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "buildingPlanApplicationDocuments");

            migrationBuilder.DropTable(
                name: "ClientConfigurations");

            migrationBuilder.DropTable(
                name: "Inspection");

            migrationBuilder.DropTable(
                name: "licenseDocuments");

            migrationBuilder.DropTable(
                name: "LogEntries");

            migrationBuilder.DropTable(
                name: "PropertyOwner");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "WorkflowLog");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Licenses");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "buildingPlanApplications");

            migrationBuilder.DropTable(
                name: "Lookups");

            migrationBuilder.DropTable(
                name: "LookupCategories");
        }
    }
}
