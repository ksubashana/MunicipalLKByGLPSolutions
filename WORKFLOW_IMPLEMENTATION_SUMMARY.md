# Building Plan Workflow Orchestration - Implementation Summary

## Objective
Implement workflow orchestration methods to link child workflow entities (Assignment, SiteInspection, PlanningCommitteeReview) to parent BuildingPlanApplication, update workflow status, and maintain audit trail via WorkflowLog entries.

## Implementation Overview

This implementation follows Clean Architecture and DDD principles, using CQRS pattern with MediatR for command handling.

### Architecture Layers Modified

#### 1. Domain Layer (`MuniLK.Domain`)
**File:** `Entities/ConstructionApplicationBase.cs`

Added three optional foreign key properties to `BuildingPlanApplication`:
```csharp
public Guid? AssignmentId { get; set; }
public Assignment? Assignment { get; set; }

public Guid? SiteInspectionId { get; set; }
public SiteInspection? SiteInspection { get; set; }

public Guid? PlanningCommitteeReviewId { get; set; }
public PlanningCommitteeReview? PlanningCommitteeReview { get; set; }
```

#### 2. Infrastructure Layer (`MuniLK.Infrastructure`)
**Files Modified:**
- `Data/Configurations/BuildingPlanApplicationConfiguration.cs` - Added EF Core relationship configuration
- `Data/Configurations/PlanningCommitteeReviewConfiguration.cs` - Removed conflicting relationship
- `Migrations/20251012051125_AddWorkflowForeignKeys.cs` - Database schema migration

**Migration Details:**
- Adds three nullable foreign key columns to `buildingPlanApplications` table
- Creates unique indexes with `IS NOT NULL` filter for optional relationships
- Configures `RESTRICT` delete behavior to prevent cascading deletes
- Drops old many-to-one relationships from child tables

#### 3. Application Layer (`MuniLK.Application`)
**Files Created:**

1. `BuildingAndPlanning/Commands/WorkflowOrchestrationCommands.cs`
   - Three command records using modern C# record syntax
   - Each accepts parent and child IDs, optional remarks, and assigned user

2. `BuildingAndPlanning/Commands/WorkflowOrchestrationCommandHandler.cs`
   - Single handler implementing all three workflow operations
   - Follows transaction pattern with UnitOfWork
   - Creates comprehensive WorkflowLog entries

### Three Workflow Methods Implemented

#### 1. AssignInspectionWorkflowAsync
**Purpose:** Link an Assignment to BuildingPlanApplication when inspector is assigned

**Process:**
1. Fetch BuildingPlanApplication by ID
2. Set `application.AssignmentId = assignmentId`
3. Update status to `BuildingAndPlanSteps.AssignInspector`
4. Add WorkflowLog with action "Inspector Assigned"
5. Save all changes transactionally

**Usage:**
```csharp
await _mediator.Send(new AssignInspectionWorkflowCommand(
    buildingPlanId, assignmentId, remarks, inspectorUserId));
```

#### 2. CreateSiteInspectionWorkflowAsync
**Purpose:** Link a SiteInspection to BuildingPlanApplication when inspection is completed

**Process:**
1. Fetch BuildingPlanApplication by ID
2. Set `application.SiteInspectionId = siteInspectionId`
3. Update status to `BuildingAndPlanSteps.ToReview`
4. Add WorkflowLog with action "Site Inspection Completed"
5. Save all changes transactionally

**Usage:**
```csharp
await _mediator.Send(new CreateSiteInspectionWorkflowCommand(
    buildingPlanId, siteInspectionId, remarks, reviewerUserId));
```

#### 3. CreatePlanningCommitteeReviewWorkflowAsync
**Purpose:** Link a PlanningCommitteeReview to BuildingPlanApplication when review is scheduled

**Process:**
1. Fetch BuildingPlanApplication by ID
2. Set `application.PlanningCommitteeReviewId = reviewId`
3. Update status to `BuildingAndPlanSteps.PlanningCommitteeReview`
4. Add WorkflowLog with action "Planning Committee Review Scheduled"
5. Save all changes transactionally

**Usage:**
```csharp
await _mediator.Send(new CreatePlanningCommitteeReviewWorkflowCommand(
    buildingPlanId, reviewId, remarks, null));
```

## Key Design Decisions

### 1. Separation of Concerns
- Child entity creation remains in existing generic handlers
- Workflow orchestration is separate, focused responsibility
- Single Responsibility Principle maintained

### 2. Transaction Safety
- All changes (FK update, status change, log creation) in single transaction
- Uses UnitOfWork pattern for consistency
- Automatic rollback on any failure

### 3. Audit Trail
- Every workflow operation creates WorkflowLog entry
- Captures: previous status, new status, action, remarks, user, role, timestamp
- Maintains complete history for compliance

### 4. Type Safety
- Entity Framework Core enforces FK constraints
- Compile-time type checking for all IDs (Guid)
- Nullable FK properties allow optional relationships

### 5. Clean Architecture
- Domain entities have no framework dependencies
- Infrastructure handles persistence concerns
- Application layer orchestrates business logic
- Commands are immutable records

## Workflow Status Transitions

| Workflow Method | Updates Status To |
|----------------|------------------|
| AssignInspectionWorkflow | `BuildingAndPlanSteps.AssignInspector` |
| CreateSiteInspectionWorkflow | `BuildingAndPlanSteps.ToReview` |
| CreatePlanningCommitteeReviewWorkflow | `BuildingAndPlanSteps.PlanningCommitteeReview` |

## Dependencies Injected

The `WorkflowOrchestrationCommandHandler` requires:
1. `IBuildingPlanRepository` - Load and persist BuildingPlanApplication
2. `ICurrentUserService` - Get current user ID and roles
3. `IWorkflowService` - Create WorkflowLog entries

These are resolved via dependency injection configured in the startup/program file.

## Error Handling

All methods return `Result` objects:
- `Result.Success()` on successful completion
- `Result.Failure(message)` with descriptive error message

Example error scenarios:
- BuildingPlanApplication not found
- Database save fails
- Validation failures

## Testing Considerations

To test these methods:
1. Mock `IBuildingPlanRepository` to return test BuildingPlanApplication
2. Mock `ICurrentUserService` to return test user data
3. Mock `IWorkflowService` to verify WorkflowLog creation
4. Verify FK is set correctly
5. Verify status is updated
6. Verify UnitOfWork.SaveChangesAsync is called once

## Integration Pattern

**Two-Step Process:**
```csharp
// Step 1: Create child entity (existing handler)
var childId = await _mediator.Send(new CreateChildEntityCommand(...));

// Step 2: Orchestrate workflow (new handler)
var result = await _mediator.Send(new WorkflowCommand(parentId, childId, ...));
```

This separation allows:
- Independent testing of child creation
- Flexible reuse of child handlers
- Clear workflow orchestration boundary

## Files Added/Modified

### Domain
- ✅ `MuniLK.Domain/Entities/ConstructionApplicationBase.cs` (modified)

### Infrastructure  
- ✅ `MuniLK.Infrastructure/Data/Configurations/BuildingPlanApplicationConfiguration.cs` (modified)
- ✅ `MuniLK.Infrastructure/Data/Configurations/PlanningCommitteeReviewConfiguration.cs` (modified)
- ✅ `MuniLK.Infrastructure/Migrations/20251012051125_AddWorkflowForeignKeys.cs` (new)
- ✅ `MuniLK.Infrastructure/Migrations/20251012051125_AddWorkflowForeignKeys.Designer.cs` (new)
- ✅ `MuniLK.Infrastructure/Migrations/MuniLKDbContextModelSnapshot.cs` (updated)

### Application
- ✅ `MuniLK.Application/BuildingAndPlanning/Commands/WorkflowOrchestrationCommands.cs` (new)
- ✅ `MuniLK.Application/BuildingAndPlanning/Commands/WorkflowOrchestrationCommandHandler.cs` (new)

### Documentation
- ✅ `WORKFLOW_ORCHESTRATION.md` (new)
- ✅ `BuildingPlanWorkflowExampleController.cs` (new - example only)
- ✅ `WORKFLOW_IMPLEMENTATION_SUMMARY.md` (this file)

## Build Status

✅ All projects build successfully with zero errors
- MuniLK.Domain: Build succeeded (0 errors)
- MuniLK.Application: Build succeeded (0 errors)  
- MuniLK.Infrastructure: Build succeeded (0 errors)

## Next Steps for Integration

1. **Run Database Migration**
   ```bash
   dotnet ef database update --project MuniLK.Infrastructure --startup-project MuniLK.API
   ```

2. **Create API Endpoints**
   - Add controller methods to expose workflow commands
   - See `BuildingPlanWorkflowExampleController.cs` for reference

3. **Update UI/Frontend**
   - Add buttons/forms to trigger workflow operations
   - Handle success/failure responses

4. **Add Integration Tests**
   - Test full workflow from child creation to parent update
   - Verify WorkflowLog entries are created correctly

5. **Documentation**
   - Update user manuals with new workflow processes
   - Add API documentation (Swagger/OpenAPI)

## Conclusion

This implementation provides a clean, maintainable solution for orchestrating Building Plan workflow operations. It follows best practices for Clean Architecture, DDD, and CQRS patterns, ensuring the codebase remains testable, scalable, and easy to understand.

The separation between child entity creation and workflow orchestration provides flexibility while maintaining transaction safety and comprehensive audit trails.
