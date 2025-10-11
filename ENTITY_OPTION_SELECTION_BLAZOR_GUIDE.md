# Blazor Integration Guide for Entity Option Selection

## Overview
This guide demonstrates how to integrate the Entity Option Selection pattern with Blazor forms, specifically showing how to use `SfMultiSelect` controls to save and load checkbox/multi-select data for entities like SiteInspection.

## Key Concepts
- **OptionGroup**: A collection of related options (e.g., "Clearance Types", "Inspection Categories")
- **OptionItem**: Individual selectable items within a group (e.g., "Fire Clearance", "CEA Clearance")
- **EntityOptionSelection**: The saved selections for a specific entity instance

## API Endpoints

### GET: Load Existing Selections
```
GET /api/Inspection/{inspectionId}/options?moduleId={moduleId}
```

**Response:**
```json
{
  "inspectionId": "guid",
  "moduleId": "guid",
  "selectedOptionItemIds": [
    "guid1",
    "guid2",
    "guid3"
  ]
}
```

### POST: Save Selections
```
POST /api/Inspection/{inspectionId}/options
```

**Request Body:**
```json
{
  "entityId": "guid",
  "entityType": "SiteInspection",
  "moduleId": "guid",
  "optionItemIds": [
    "guid1",
    "guid2",
    "guid3"
  ]
}
```

### GET: Load Existing Site Inspection
```
GET /api/Inspection/{inspectionId}
```

**Response:**
```json
{
  "id": "guid",
  "applicationId": "guid",
  "inspectionId": "guid",
  "status": "Pending",
  "remarks": "string",
  "inspectionDate": "2025-01-15T00:00:00Z",
  "officersPresent": "string",
  "gpsCoordinates": "string",
  "requiredModifications": "string",
  "clearancesRequired": [...],
  "finalRecommendation": "ApproveAsSubmitted",
  ...
}
```

## Blazor Implementation Example

### 1. Define Option Data Classes

```csharp
// In your Blazor component's @code section
public class OptionItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class ClearanceTypeOption
{
    public Guid Value { get; set; }  // OptionItemId
    public string Text { get; set; } = string.Empty;  // Display name
}
```

### 2. Component Properties and State

```csharp
@code {
    [Parameter] public Guid InspectionId { get; set; }
    [Parameter] public Guid ModuleId { get; set; }
    
    // Model to bind to the form
    private SiteInspectionRequest Model = new();
    
    // Options data source for multi-select
    private List<ClearanceTypeOption> ClearanceTypeOptions = new();
    
    // Selected option IDs (bound to SfMultiSelect)
    private List<Guid> SelectedClearanceIds = new();
}
```

### 3. Load Existing Data on Initialization

```csharp
protected override async Task OnInitializedAsync()
{
    // Load option items from your database/API to populate the dropdown
    await LoadClearanceTypeOptionsAsync();
    
    // Load existing inspection data
    if (InspectionId != Guid.Empty)
    {
        await LoadExistingInspectionAsync(InspectionId);
        
        // Load saved option selections
        await LoadExistingOptionSelectionsAsync(InspectionId);
    }
}

private async Task LoadClearanceTypeOptionsAsync()
{
    try
    {
        var client = HttpClientFactory.CreateClient("AuthorizedClient");
        
        // You would call your API to get all available OptionItems for the "Clearances" group
        var response = await client.GetStringAsync("api/OptionItems/group/clearances");
        var options = System.Text.Json.JsonSerializer.Deserialize<List<OptionItemDto>>(response,
            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        ClearanceTypeOptions = options?.Select(o => new ClearanceTypeOption 
        { 
            Value = o.Id, 
            Text = o.Name 
        }).ToList() ?? new();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading clearance options: {ex.Message}");
        ClearanceTypeOptions = new();
    }
}

private async Task LoadExistingInspectionAsync(Guid inspectionId)
{
    try
    {
        var client = HttpClientFactory.CreateClient("AuthorizedClient");
        var response = await client.GetStringAsync($"api/Inspection/{inspectionId}");
        var existingInspection = System.Text.Json.JsonSerializer.Deserialize<SiteInspectionResponse>(response,
            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        if (existingInspection != null)
        {
            // Populate form from existing inspection
            Model.Status = existingInspection.Status;
            Model.InspectionDate = existingInspection.InspectionDate;
            Model.OfficersPresent = existingInspection.OfficersPresent;
            Model.GpsCoordinates = existingInspection.GpsCoordinates;
            Model.Remarks = existingInspection.Remarks;
            Model.FinalRecommendation = existingInspection.FinalRecommendation;
            // ... populate other fields
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading inspection: {ex.Message}");
    }
}

private async Task LoadExistingOptionSelectionsAsync(Guid inspectionId)
{
    try
    {
        var client = HttpClientFactory.CreateClient("AuthorizedClient");
        var response = await client.GetStringAsync(
            $"api/Inspection/{inspectionId}/options?moduleId={ModuleId}");
        
        var result = System.Text.Json.JsonSerializer.Deserialize<OptionSelectionResponse>(response,
            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        if (result?.SelectedOptionItemIds != null)
        {
            // Set the selected IDs - this will automatically check the boxes in SfMultiSelect
            SelectedClearanceIds = result.SelectedOptionItemIds;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading option selections: {ex.Message}");
    }
}

private class OptionSelectionResponse
{
    public Guid InspectionId { get; set; }
    public Guid ModuleId { get; set; }
    public List<Guid> SelectedOptionItemIds { get; set; } = new();
}
```

### 4. Razor Markup with SfMultiSelect

```razor
<div class="col-md-6 mb-3">
    <label class="form-label">Clearances Required</label>
    <SfMultiSelect TValue="List<Guid>" 
                   TItem="ClearanceTypeOption" 
                   @bind-Value="SelectedClearanceIds" 
                   DataSource="@ClearanceTypeOptions" 
                   Placeholder="Select required clearances" 
                   CssClass="form-control"
                   Mode="@VisualMode.CheckBox"
                   ShowSelectAll="true"
                   ClosePopupOnSelect="false">
        <MultiSelectFieldSettings Value="Value" Text="Text" />
    </SfMultiSelect>
</div>
```

### 5. Save Selections on Form Submit

```csharp
private async Task HandleValidSubmit()
{
    IsSubmitting = true;
    try
    {
        var client = HttpClientFactory.CreateClient("AuthorizedClient");
        
        // First, save the inspection data
        var inspectionUrl = $"api/BuildingPlans/{ApplicationId}/complete-site-inspection";
        var inspectionResponse = await client.PostAsJsonAsync(inspectionUrl, Model);
        
        if (!inspectionResponse.IsSuccessStatusCode)
        {
            var error = await inspectionResponse.Content.ReadAsStringAsync();
            await ShowErrorToast($"Failed to submit inspection: {error}");
            return;
        }
        
        // Then, save the option selections
        await SaveOptionSelectionsAsync(InspectionId);
        
        await ShowSuccessToast("Site inspection submitted successfully");
        
        if (OnSubmit.HasDelegate)
        {
            await OnSubmit.InvokeAsync(Model);
        }
    }
    catch (Exception ex)
    {
        await ShowErrorToast($"Error submitting form: {ex.Message}");
    }
    finally
    {
        IsSubmitting = false;
    }
}

private async Task SaveOptionSelectionsAsync(Guid inspectionId)
{
    try
    {
        var client = HttpClientFactory.CreateClient("AuthorizedClient");
        
        var request = new SaveEntityOptionSelectionsRequest
        {
            EntityId = inspectionId,
            EntityType = "SiteInspection",
            ModuleId = ModuleId,
            OptionItemIds = SelectedClearanceIds ?? new List<Guid>()
        };
        
        var response = await client.PostAsJsonAsync(
            $"api/Inspection/{inspectionId}/options", 
            request);
        
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Failed to save option selections: {error}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error saving option selections: {ex.Message}");
    }
}

public class SaveEntityOptionSelectionsRequest
{
    public Guid EntityId { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public Guid ModuleId { get; set; }
    public List<Guid> OptionItemIds { get; set; } = new();
}
```

## Complete Example: SiteInspectionForm.razor

### Markup Section
```razor
@page "/inspections/site/{InspectionId:guid}"
@using Syncfusion.Blazor.DropDowns
@using Syncfusion.Blazor.Inputs
@inject IHttpClientFactory HttpClientFactory

<EditForm Model="@Model" OnValidSubmit="HandleValidSubmit">
    <DataAnnotationsValidator />
    <ValidationSummary class="text-danger" />
    
    <!-- Existing form fields for inspection data -->
    <div class="row mb-4">
        <div class="col-md-6">
            <label class="form-label">Inspection Status <span class="text-danger">*</span></label>
            <SfDropDownList TValue="InspectionStatus" 
                          TItem="InspectionStatusOption" 
                          @bind-Value="Model.Status" 
                          DataSource="@InspectionStatusOptions" 
                          Placeholder="Select Status" 
                          CssClass="form-control">
                <DropDownListFieldSettings Value="Value" Text="Text" />
            </SfDropDownList>
        </div>
        
        <div class="col-md-6">
            <label class="form-label">Inspection Date <span class="text-danger">*</span></label>
            <SfDatePicker TValue="DateTime" 
                         @bind-Value="Model.InspectionDate"
                         CssClass="form-control" />
        </div>
    </div>
    
    <!-- Multi-select for clearances using Entity Option Selection -->
    <div class="row mb-4">
        <div class="col-md-6">
            <label class="form-label">Clearances Required</label>
            <SfMultiSelect TValue="List<Guid>" 
                         TItem="ClearanceTypeOption" 
                         @bind-Value="SelectedClearanceIds" 
                         DataSource="@ClearanceTypeOptions" 
                         Placeholder="Select required clearances" 
                         CssClass="form-control"
                         Mode="@VisualMode.CheckBox"
                         ShowSelectAll="true"
                         ClosePopupOnSelect="false">
                <MultiSelectFieldSettings Value="Value" Text="Text" />
            </SfMultiSelect>
        </div>
    </div>
    
    <!-- Submit button -->
    <div class="row">
        <div class="col-12">
            <button type="submit" class="btn btn-primary" disabled="@IsSubmitting">
                @if (IsSubmitting)
                {
                    <span class="spinner-border spinner-border-sm me-2"></span>
                }
                Submit Inspection
            </button>
        </div>
    </div>
</EditForm>
```

### Code Section (Complete)
```csharp
@code {
    [Parameter] public Guid InspectionId { get; set; }
    [Parameter] public Guid ApplicationId { get; set; }
    [Parameter] public Guid ModuleId { get; set; }
    [Parameter] public EventCallback<SiteInspectionRequest> OnSubmit { get; set; }
    
    private SiteInspectionRequest Model = new();
    private bool IsSubmitting = false;
    
    // Multi-select data
    private List<ClearanceTypeOption> ClearanceTypeOptions = new();
    private List<Guid> SelectedClearanceIds = new();
    
    // Dropdown data
    private List<InspectionStatusOption> InspectionStatusOptions = new()
    {
        new() { Value = InspectionStatus.Pending, Text = "Pending" },
        new() { Value = InspectionStatus.Approve, Text = "Approve" },
        new() { Value = InspectionStatus.Reject, Text = "Reject" }
    };
    
    protected override async Task OnInitializedAsync()
    {
        Model.ApplicationId = ApplicationId;
        Model.InspectionDate = DateTime.Today;
        
        // Load all dropdown options
        await LoadClearanceTypeOptionsAsync();
        
        // Load existing data if editing
        if (InspectionId != Guid.Empty)
        {
            await LoadExistingInspectionAsync(InspectionId);
            await LoadExistingOptionSelectionsAsync(InspectionId);
        }
    }
    
    private async Task LoadClearanceTypeOptionsAsync()
    {
        try
        {
            var client = HttpClientFactory.CreateClient("AuthorizedClient");
            var response = await client.GetStringAsync("api/OptionItems/group/clearances");
            var options = System.Text.Json.JsonSerializer.Deserialize<List<OptionItemDto>>(response,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            ClearanceTypeOptions = options?.Select(o => new ClearanceTypeOption 
            { 
                Value = o.Id, 
                Text = o.Name 
            }).ToList() ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading clearance options: {ex.Message}");
            ClearanceTypeOptions = new();
        }
    }
    
    private async Task LoadExistingInspectionAsync(Guid inspectionId)
    {
        try
        {
            var client = HttpClientFactory.CreateClient("AuthorizedClient");
            var response = await client.GetStringAsync($"api/Inspection/{inspectionId}");
            var existingInspection = System.Text.Json.JsonSerializer.Deserialize<SiteInspectionResponse>(response,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            if (existingInspection != null)
            {
                Model.Status = existingInspection.Status;
                Model.InspectionDate = existingInspection.InspectionDate;
                Model.OfficersPresent = existingInspection.OfficersPresent;
                Model.GpsCoordinates = existingInspection.GpsCoordinates;
                Model.Remarks = existingInspection.Remarks;
                Model.FinalRecommendation = existingInspection.FinalRecommendation;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading inspection: {ex.Message}");
        }
    }
    
    private async Task LoadExistingOptionSelectionsAsync(Guid inspectionId)
    {
        try
        {
            var client = HttpClientFactory.CreateClient("AuthorizedClient");
            var response = await client.GetStringAsync(
                $"api/Inspection/{inspectionId}/options?moduleId={ModuleId}");
            
            var result = System.Text.Json.JsonSerializer.Deserialize<OptionSelectionResponse>(response,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            if (result?.SelectedOptionItemIds != null)
            {
                SelectedClearanceIds = result.SelectedOptionItemIds;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading option selections: {ex.Message}");
        }
    }
    
    private async Task HandleValidSubmit()
    {
        IsSubmitting = true;
        try
        {
            var client = HttpClientFactory.CreateClient("AuthorizedClient");
            
            // Save inspection data
            var inspectionUrl = $"api/BuildingPlans/{ApplicationId}/complete-site-inspection";
            var inspectionResponse = await client.PostAsJsonAsync(inspectionUrl, Model);
            
            if (!inspectionResponse.IsSuccessStatusCode)
            {
                var error = await inspectionResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to submit inspection: {error}");
                return;
            }
            
            // Save option selections
            await SaveOptionSelectionsAsync(InspectionId);
            
            Console.WriteLine("Site inspection submitted successfully");
            
            if (OnSubmit.HasDelegate)
            {
                await OnSubmit.InvokeAsync(Model);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error submitting form: {ex.Message}");
        }
        finally
        {
            IsSubmitting = false;
        }
    }
    
    private async Task SaveOptionSelectionsAsync(Guid inspectionId)
    {
        try
        {
            var client = HttpClientFactory.CreateClient("AuthorizedClient");
            
            var request = new SaveEntityOptionSelectionsRequest
            {
                EntityId = inspectionId,
                EntityType = "SiteInspection",
                ModuleId = ModuleId,
                OptionItemIds = SelectedClearanceIds ?? new List<Guid>()
            };
            
            var response = await client.PostAsJsonAsync(
                $"api/Inspection/{inspectionId}/options", 
                request);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to save option selections: {error}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving option selections: {ex.Message}");
        }
    }
    
    // Helper classes
    public class ClearanceTypeOption
    {
        public Guid Value { get; set; }
        public string Text { get; set; } = string.Empty;
    }
    
    public class InspectionStatusOption
    {
        public InspectionStatus Value { get; set; }
        public string Text { get; set; } = string.Empty;
    }
    
    public class OptionItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    
    public class OptionSelectionResponse
    {
        public Guid InspectionId { get; set; }
        public Guid ModuleId { get; set; }
        public List<Guid> SelectedOptionItemIds { get; set; } = new();
    }
    
    public class SaveEntityOptionSelectionsRequest
    {
        public Guid EntityId { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public Guid ModuleId { get; set; }
        public List<Guid> OptionItemIds { get; set; } = new();
    }
}
```

## Key Points

1. **Separation of Concerns**: The option selection logic is completely separate from the main entity (SiteInspection). This allows you to reuse the same pattern for any entity type.

2. **Transaction Safety**: The API endpoint uses transactions to ensure that the delete-and-insert operation is atomic.

3. **Validation**: The system validates that all OptionItemIds exist before saving.

4. **Flexibility**: The same pattern works for any multi-select scenario - not just clearances. You can use it for permits, categories, tags, etc.

5. **Multi-Tenancy**: All entities respect the TenantId through the IHasTenant interface.

## Database Seeding Example

To use this system, you need to seed OptionGroups and OptionItems:

```sql
-- Create a group for clearances
INSERT INTO OptionGroup (Id, Name, TenantId) 
VALUES (NEWID(), 'Clearance Types', @TenantId);

-- Get the group ID
DECLARE @GroupId UNIQUEIDENTIFIER = (SELECT Id FROM OptionGroup WHERE Name = 'Clearance Types' AND TenantId = @TenantId);

-- Add option items
INSERT INTO OptionItem (Id, GroupId, Name, TenantId) VALUES
(NEWID(), @GroupId, 'CEA - Central Environmental Authority', @TenantId),
(NEWID(), @GroupId, 'Coast Conservation Department', @TenantId),
(NEWID(), @GroupId, 'NBRO - National Building Research Organization', @TenantId),
(NEWID(), @GroupId, 'Fire Department', @TenantId),
(NEWID(), @GroupId, 'Other', @TenantId);
```

## Usage in Other Modules

The same pattern can be applied to any module. For example, for a Permit entity:

```csharp
// In PermitController
[HttpGet("{permitId:guid}/options")]
public async Task<IActionResult> GetPermitOptions(Guid permitId, [FromQuery] Guid moduleId)
{
    var optionItemIds = await _mediator.Send(
        new GetEntityOptionSelectionsQuery(permitId, "Permit", moduleId));
    return Ok(new { PermitId = permitId, SelectedOptionItemIds = optionItemIds });
}

[HttpPost("{permitId:guid}/options")]
public async Task<IActionResult> SavePermitOptions(Guid permitId, [FromBody] SaveEntityOptionSelectionsRequest request)
{
    var command = new SaveEntityOptionSelectionsCommand(
        permitId, "Permit", request.ModuleId, request.OptionItemIds);
    var result = await _mediator.Send(command);
    return result.Succeeded ? Ok(result.Data) : BadRequest(result.Error);
}
```

This ensures consistency across the entire application.
