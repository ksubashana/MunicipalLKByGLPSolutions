# Entity Option Selection Pattern - Implementation Summary

## Overview
This document provides a summary of the Entity Option Selection pattern implementation for the Municipal LK system. This pattern provides a reusable solution for saving and loading multi-select/checkbox selections for any entity type without creating dedicated linking tables for each module.

## Database Schema

### Tables Created
1. **OptionGroup** - Groups related options together (e.g., "Clearance Types")
2. **OptionItem** - Individual selectable items within a group (e.g., "Fire Clearance")
3. **EntityOptionSelection** - Stores the actual selections for any entity

### Migration Details
- **Migration Name**: `20251011023552_AddEntityOptionSelectionTables`
- **Location**: `/MuniLK.Infrastructure/Migrations/`

### Schema Features
- All entities support multi-tenancy via `IHasTenant` interface
- Composite index on `EntityId`, `EntityType`, and `ModuleId` for efficient queries
- Foreign key constraints with proper cascade/restrict behavior
- All IDs are GUID-based for consistency

## Architecture Components

### Domain Layer (`MuniLK.Domain`)
```
/Entities/
  - OptionGroup.cs
  - OptionItem.cs
  - EntityOptionSelection.cs
```

### Application Layer (`MuniLK.Application`)
```
/BuildingAndPlanning/
  /Commands/
    - EntityOptionSelectionCommands.cs (Save, Delete)
  /Queries/
    - GetEntityOptionSelectionsQuery.cs
    - GetSiteInspectionQuery.cs
  /Handlers/
    - EntityOptionSelectionCommandHandlers.cs
    - GetEntityOptionSelectionsQueryHandler.cs
    - GetSiteInspectionQueryHandler.cs
  /Interfaces/
    - IEntityOptionSelectionRepository.cs
    - IEntityOptionSelectionService.cs
  /Services/
    - EntityOptionSelectionService.cs
  /DTOs/
    - EntityOptionSelectionDtos.cs
```

### Infrastructure Layer (`MuniLK.Infrastructure`)
```
/Data/Configurations/
  - OptionGroupConfiguration.cs
  - OptionItemConfiguration.cs
  - EntityOptionSelectionConfiguration.cs
/BuildingAndPlanning/
  - EntityOptionSelectionRepository.cs
```

### API Layer (`MuniLK.API`)
```
/Controllers/
  - InspectionController.cs (updated with new endpoints)
Program.cs (service registration)
```

## API Endpoints

### GET /api/Inspection/{inspectionId}
Load existing site inspection data including all fields.

**Response:**
```json
{
  "id": "guid",
  "applicationId": "guid",
  "inspectionId": "guid",
  "status": "Pending",
  "inspectionDate": "2025-01-15T00:00:00Z",
  ...
}
```

### GET /api/Inspection/{inspectionId}/options?moduleId={moduleId}
Load saved option selections for a specific inspection.

**Response:**
```json
{
  "inspectionId": "guid",
  "moduleId": "guid",
  "selectedOptionItemIds": ["guid1", "guid2", "guid3"]
}
```

### POST /api/Inspection/{inspectionId}/options
Save option selections (replaces existing selections).

**Request:**
```json
{
  "entityId": "guid",
  "entityType": "SiteInspection",
  "moduleId": "guid",
  "optionItemIds": ["guid1", "guid2", "guid3"]
}
```

**Response:**
```json
{
  "entityId": "guid",
  "entityType": "SiteInspection",
  "moduleId": "guid",
  "selectedOptionItemIds": ["guid1", "guid2", "guid3"],
  "success": true,
  "message": "Selections saved successfully"
}
```

## Key Features

### 1. Transaction Safety
All save operations use database transactions to ensure atomicity:
- Delete existing selections
- Insert new selections
- Commit or rollback as a unit

### 2. Validation
- Validates that EntityId, EntityType, and ModuleId are provided
- Validates that all OptionItemIds exist in the database before saving
- Returns meaningful error messages

### 3. Multi-Tenancy Support
- All entities implement `IHasTenant` interface
- TenantId automatically set via DbContext override
- Query filters applied automatically

### 4. Flexibility
The same pattern works for any entity type:
- SiteInspection
- Permit
- BuildingPlan
- License
- Any future entity

Just change the `EntityType` parameter in the API calls.

### 5. Clean Architecture
- Follows CQRS pattern with MediatR
- Repository pattern for data access
- Service layer for business logic
- Clear separation of concerns

## Service Registration

In `Program.cs`:
```csharp
builder.Services.AddScoped<IEntityOptionSelectionRepository, EntityOptionSelectionRepository>();
builder.Services.AddScoped<IEntityOptionSelectionService, EntityOptionSelectionService>();
```

## Usage Examples

### Example 1: Site Inspection Clearances
```csharp
// Save clearances required for a site inspection
var command = new SaveEntityOptionSelectionsCommand(
    inspectionId,
    "SiteInspection",
    moduleId,
    new List<Guid> { ceaId, nbro Id, fireDeptId }
);
var result = await _mediator.Send(command);
```

### Example 2: Permit Categories
```csharp
// Save categories for a permit
var command = new SaveEntityOptionSelectionsCommand(
    permitId,
    "Permit",
    moduleId,
    new List<Guid> { category1Id, category2Id }
);
var result = await _mediator.Send(command);
```

## Database Seeding

To use the system, seed OptionGroups and OptionItems:

```sql
-- Example: Clearance Types
DECLARE @TenantId UNIQUEIDENTIFIER = '...';
DECLARE @GroupId UNIQUEIDENTIFIER = NEWID();

INSERT INTO OptionGroups (Id, Name, TenantId) 
VALUES (@GroupId, 'Clearance Types', @TenantId);

INSERT INTO OptionItems (Id, GroupId, Name, TenantId) VALUES
(NEWID(), @GroupId, 'CEA - Central Environmental Authority', @TenantId),
(NEWID(), @GroupId, 'Coast Conservation Department', @TenantId),
(NEWID(), @GroupId, 'NBRO - National Building Research Organization', @TenantId),
(NEWID(), @GroupId, 'Fire Department', @TenantId),
(NEWID(), @GroupId, 'Other', @TenantId);
```

## Testing

### Build Status
✅ All projects build successfully with 0 errors
- Warnings present are pre-existing in the codebase
- New code follows existing patterns and conventions

### Migration Status
✅ Migration created successfully
- Tables: OptionGroups, OptionItems, EntityOptionSelections
- Indexes: Composite index on (EntityId, EntityType, ModuleId)
- Foreign keys: Proper cascade/restrict behavior

## Documentation

### Blazor Integration Guide
Complete guide available in: `ENTITY_OPTION_SELECTION_BLAZOR_GUIDE.md`

Includes:
- Complete Blazor component examples
- Loading and saving data patterns
- SfMultiSelect integration
- Error handling
- Transaction management

## Extensibility

### Adding New Option Groups
1. Seed new OptionGroup in database
2. Add OptionItems for that group
3. Use existing API endpoints (no code changes needed)

### Supporting New Entity Types
1. Add API endpoints in relevant controller
2. Call existing commands/queries with new EntityType
3. No changes to domain/infrastructure layer needed

### Adding Additional Features
The pattern can be extended to support:
- Hierarchical options (parent-child relationships)
- Option metadata (descriptions, icons, ordering)
- Conditional options (based on other selections)
- Option history tracking

## Best Practices

1. **Always provide ModuleId**: Ensures selections are scoped to the correct module
2. **Use transactions**: Already implemented in command handlers
3. **Validate input**: Command handlers include validation
4. **Load existing data**: Check for existing selections before saving
5. **Error handling**: Wrap API calls in try-catch blocks
6. **Consistent naming**: Use descriptive EntityType values (e.g., "SiteInspection" not "Inspection")

## Future Enhancements

Potential improvements:
1. Add audit fields (CreatedDate, CreatedBy, ModifiedDate, ModifiedBy)
2. Implement soft delete for EntityOptionSelection
3. Add caching for frequently accessed OptionItems
4. Create admin UI for managing OptionGroups and OptionItems
5. Add bulk operations endpoint for multiple entities
6. Implement option validation rules (required, max selections, etc.)

## Support

For issues or questions:
1. Check the Blazor integration guide
2. Review the example implementations
3. Verify database seeding is complete
4. Check API endpoint responses for error messages

## Conclusion

The Entity Option Selection pattern provides a robust, reusable solution for managing multi-select data across the entire application. It follows Clean Architecture principles, supports multi-tenancy, and is fully documented with examples.

The implementation is production-ready and can be used immediately for SiteInspection clearances and extended to other modules as needed.
