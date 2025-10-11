# Entity Option Selection - Quick Reference

## Quick Start

### 1. Database Setup
Run the migration:
```bash
dotnet ef database update --project MuniLK.Infrastructure --startup-project MuniLK.API --context MuniLKDbContext
```

### 2. Seed Data
```sql
DECLARE @TenantId UNIQUEIDENTIFIER = '...'; -- Your tenant ID
DECLARE @GroupId UNIQUEIDENTIFIER = NEWID();

-- Create option group
INSERT INTO OptionGroups (Id, Name, TenantId) 
VALUES (@GroupId, 'Clearance Types', @TenantId);

-- Add option items
INSERT INTO OptionItems (Id, GroupId, Name, TenantId) VALUES
(NEWID(), @GroupId, 'CEA Clearance', @TenantId),
(NEWID(), @GroupId, 'Fire Department', @TenantId),
(NEWID(), @GroupId, 'NBRO', @TenantId);
```

### 3. API Calls

#### Get Selections
```http
GET /api/Inspection/{inspectionId}/options?moduleId={moduleId}
Authorization: Bearer {token}
```

#### Save Selections
```http
POST /api/Inspection/{inspectionId}/options
Authorization: Bearer {token}
Content-Type: application/json

{
  "entityId": "guid",
  "entityType": "SiteInspection",
  "moduleId": "guid",
  "optionItemIds": ["guid1", "guid2"]
}
```

### 4. Blazor Integration

#### Component Properties
```csharp
[Parameter] public Guid InspectionId { get; set; }
[Parameter] public Guid ModuleId { get; set; }

private List<ClearanceTypeOption> ClearanceOptions = new();
private List<Guid> SelectedIds = new();
```

#### Load Data
```csharp
protected override async Task OnInitializedAsync()
{
    await LoadOptionsAsync();
    await LoadSelectionsAsync();
}

private async Task LoadSelectionsAsync()
{
    var client = HttpClientFactory.CreateClient("AuthorizedClient");
    var json = await client.GetStringAsync(
        $"api/Inspection/{InspectionId}/options?moduleId={ModuleId}");
    var result = JsonSerializer.Deserialize<SelectionsResponse>(json);
    SelectedIds = result.SelectedOptionItemIds;
}
```

#### Razor Markup
```razor
<SfMultiSelect TValue="List<Guid>" 
               TItem="ClearanceTypeOption" 
               @bind-Value="SelectedIds" 
               DataSource="@ClearanceOptions" 
               Mode="@VisualMode.CheckBox">
    <MultiSelectFieldSettings Value="Value" Text="Text" />
</SfMultiSelect>
```

#### Save Data
```csharp
private async Task SaveAsync()
{
    var client = HttpClientFactory.CreateClient("AuthorizedClient");
    var request = new {
        entityId = InspectionId,
        entityType = "SiteInspection",
        moduleId = ModuleId,
        optionItemIds = SelectedIds
    };
    await client.PostAsJsonAsync($"api/Inspection/{InspectionId}/options", request);
}
```

## Common Patterns

### Pattern 1: Load-Edit-Save
```csharp
// OnInitialized
await LoadExistingData();

// OnSubmit
await SaveData();
await SaveSelections();
```

### Pattern 2: Using Service Layer
```csharp
// Inject service
@inject IEntityOptionSelectionService OptionService

// Load
var selections = await OptionService.GetSelectionsAsync(
    entityId, "SiteInspection", moduleId);

// Save
await OptionService.SaveSelectionsAsync(
    entityId, "SiteInspection", moduleId, optionItemIds);
```

### Pattern 3: Using MediatR Directly
```csharp
// Inject mediator
@inject IMediator Mediator

// Load
var selections = await Mediator.Send(
    new GetEntityOptionSelectionsQuery(entityId, "SiteInspection", moduleId));

// Save
var result = await Mediator.Send(
    new SaveEntityOptionSelectionsCommand(entityId, "SiteInspection", moduleId, ids));
```

## Entity Types

Use consistent EntityType values:
- `"SiteInspection"` - For site inspections
- `"Permit"` - For permits
- `"BuildingPlan"` - For building plans
- `"License"` - For licenses

## Error Handling

### API Level
```csharp
try
{
    var response = await client.PostAsJsonAsync(url, request);
    if (!response.IsSuccessStatusCode)
    {
        var error = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error: {error}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Exception: {ex.Message}");
}
```

### Service Level
```csharp
var result = await Mediator.Send(command);
if (!result.Succeeded)
{
    // Handle error: result.Error
}
else
{
    // Handle success: result.Data
}
```

## Files Reference

### Domain
- `MuniLK.Domain/Entities/OptionGroup.cs`
- `MuniLK.Domain/Entities/OptionItem.cs`
- `MuniLK.Domain/Entities/EntityOptionSelection.cs`

### Application
- `MuniLK.Application/BuildingAndPlanning/Commands/EntityOptionSelectionCommands.cs`
- `MuniLK.Application/BuildingAndPlanning/Queries/GetEntityOptionSelectionsQuery.cs`
- `MuniLK.Application/BuildingAndPlanning/Handlers/EntityOptionSelectionCommandHandlers.cs`
- `MuniLK.Application/BuildingAndPlanning/Handlers/GetEntityOptionSelectionsQueryHandler.cs`

### Infrastructure
- `MuniLK.Infrastructure/Data/Configurations/OptionGroupConfiguration.cs`
- `MuniLK.Infrastructure/Data/Configurations/OptionItemConfiguration.cs`
- `MuniLK.Infrastructure/Data/Configurations/EntityOptionSelectionConfiguration.cs`
- `MuniLK.Infrastructure/BuildingAndPlanning/EntityOptionSelectionRepository.cs`

### API
- `MuniLK.API/Controllers/InspectionController.cs` (endpoints added)
- `MuniLK.API/Program.cs` (services registered)

## Documentation
- `ENTITY_OPTION_SELECTION_BLAZOR_GUIDE.md` - Complete guide with examples
- `IMPLEMENTATION_SUMMARY.md` - Architecture overview and details

## Tips

1. **Always provide ModuleId** - Ensures proper scoping
2. **Use transactions** - Already handled by command handlers
3. **Validate before save** - Check OptionItemIds exist
4. **Load on init** - Populate dropdown options first
5. **Handle errors** - Wrap API calls in try-catch
6. **Test thoroughly** - Verify load and save operations
7. **Consistent naming** - Use descriptive EntityType values

## Troubleshooting

### Issue: No options showing in dropdown
- Verify OptionItems are seeded in database
- Check API endpoint for loading options
- Verify TenantId filtering

### Issue: Selections not saving
- Check that EntityId, EntityType, ModuleId are valid
- Verify OptionItemIds exist in database
- Check API response for error messages
- Verify transaction is not rolling back

### Issue: Selections not loading
- Verify inspectionId is correct
- Check ModuleId parameter is provided
- Verify data exists in EntityOptionSelections table
- Check API endpoint returns expected format

## Support

For detailed examples and integration guide, see:
`ENTITY_OPTION_SELECTION_BLAZOR_GUIDE.md`

For architecture and implementation details, see:
`IMPLEMENTATION_SUMMARY.md`
