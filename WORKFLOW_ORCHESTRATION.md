# Building Plan Workflow Orchestration

## Overview

This implementation provides workflow orchestration methods for the Building Plan application workflow. The solution follows Clean Architecture and DDD best practices, using the Command/Query Responsibility Segregation (CQRS) pattern with MediatR.

## Architecture Components

### 1. Domain Layer Changes

**BuildingPlanApplication Entity** (`MuniLK.Domain/Entities/ConstructionApplicationBase.cs`)

Added optional foreign key properties to link workflow child entities:

```csharp
public Guid? AssignmentId { get; set; }
public Assignment? Assignment { get; set; }

public Guid? SiteInspectionId { get; set; }
public SiteInspection? SiteInspection { get; set; }

public Guid? PlanningCommitteeReviewId { get; set; }
public PlanningCommitteeReview? PlanningCommitteeReview { get; set; }
```

### 2. Infrastructure Layer Changes

**Database Migration** (`MuniLK.Infrastructure/Migrations/20251012051125_AddWorkflowForeignKeys.cs`)

- Added three nullable foreign key columns to `buildingPlanApplications` table
- Created unique indexes for each foreign key
- Configured relationships with `Restrict` delete behavior

**Entity Configuration** (`MuniLK.Infrastructure/Data/Configurations/BuildingPlanApplicationConfiguration.cs`)

- Configured one-to-one relationships between BuildingPlanApplication and workflow child entities
- Set `IsRequired(false)` for optional relationships
- Applied `OnDelete(DeleteBehavior.Restrict)` to prevent cascading deletes

### 3. Application Layer Changes

**Commands** (`MuniLK.Application/BuildingAndPlanning/Commands/WorkflowOrchestrationCommands.cs`)

Three new commands for workflow orchestration:

1. `AssignInspectionWorkflowCommand` - Links an Assignment to the application
2. `CreateSiteInspectionWorkflowCommand` - Links a SiteInspection to the application
3. `CreatePlanningCommitteeReviewWorkflowCommand` - Links a PlanningCommitteeReview to the application

**Command Handler** (`MuniLK.Application/BuildingAndPlanning/Commands/WorkflowOrchestrationCommandHandler.cs`)

Implements three workflow orchestration methods that handle:
- Updating the parent foreign key
- Updating the workflow status
- Adding a WorkflowLog entry
- Saving all changes in one transaction

## Usage Examples

### 1. Assign Inspector Workflow

After creating an Assignment entity using `CreateAssignmentCommandHandler`, orchestrate the workflow:

```csharp
// First, create the Assignment entity (handled by existing generic handler)
var assignmentId = await _mediator.Send(new CreateAssignmentCommand(...));

// Then, orchestrate the workflow to link it to the BuildingPlanApplication
var result = await _mediator.Send(new AssignInspectionWorkflowCommand(
    BuildingPlanApplicationId: buildingPlanId,
    AssignmentId: assignmentId,
    Remarks: "Inspector assigned for site verification",
    AssignedUserId: inspectorUserId
));
```

**What it does:**
- Sets `BuildingPlanApplication.AssignmentId = assignmentId`
- Updates status to `BuildingAndPlanSteps.AssignInspector`
- Creates WorkflowLog with action "Inspector Assigned"
- Saves all changes in one transaction

### 2. Site Inspection Workflow

After creating a SiteInspection entity using `CreateSiteInspectionCommandHandler`, orchestrate the workflow:

```csharp
// First, create the SiteInspection entity (handled by existing handler)
var siteInspectionId = await _mediator.Send(new CompleteSiteInspectionCommand(...));

// Then, orchestrate the workflow to link it to the BuildingPlanApplication
var result = await _mediator.Send(new CreateSiteInspectionWorkflowCommand(
    BuildingPlanApplicationId: buildingPlanId,
    SiteInspectionId: siteInspectionId,
    Remarks: "Site inspection completed successfully",
    AssignedUserId: reviewerUserId
));
```

**What it does:**
- Sets `BuildingPlanApplication.SiteInspectionId = siteInspectionId`
- Updates status to `BuildingAndPlanSteps.ToReview`
- Creates WorkflowLog with action "Site Inspection Completed"
- Saves all changes in one transaction

### 3. Planning Committee Review Workflow

After creating a PlanningCommitteeReview entity using `SavePlanningCommitteeReviewCommand`, orchestrate the workflow:

```csharp
// First, create the PlanningCommitteeReview entity (handled by existing handler)
var reviewId = await _mediator.Send(new SavePlanningCommitteeReviewCommand(...));

// Then, orchestrate the workflow to link it to the BuildingPlanApplication
var result = await _mediator.Send(new CreatePlanningCommitteeReviewWorkflowCommand(
    BuildingPlanApplicationId: buildingPlanId,
    PlanningCommitteeReviewId: reviewId,
    Remarks: "Planning committee review scheduled",
    AssignedUserId: null // Optional
));
```

**What it does:**
- Sets `BuildingPlanApplication.PlanningCommitteeReviewId = reviewId`
- Updates status to `BuildingAndPlanSteps.PlanningCommitteeReview`
- Creates WorkflowLog with action "Planning Committee Review Scheduled"
- Saves all changes in one transaction

## Workflow Status Transitions

The workflow orchestration methods update the BuildingPlanApplication status as follows:

| Method | Previous Status | New Status |
|--------|----------------|------------|
| AssignInspectionWorkflow | Any | AssignInspector |
| CreateSiteInspectionWorkflow | Any | ToReview |
| CreatePlanningCommitteeReviewWorkflow | Any | PlanningCommitteeReview |

## WorkflowLog Tracking

Each workflow operation creates a comprehensive audit trail with:

- **ApplicationId**: The BuildingPlanApplication ID
- **ActionTaken**: Human-readable description (e.g., "Inspector Assigned")
- **PreviousStatus**: Status before the workflow action
- **NewStatus**: Status after the workflow action
- **Remarks**: Optional comments about the action
- **PerformedByUserId**: User who performed the action
- **PerformedByRole**: Role(s) of the user
- **AssignedToUserId**: User assigned to the next task (optional)
- **IsSystemGenerated**: false (these are user actions)
- **PerformedAt**: Timestamp of the action

## Transaction Management

All workflow operations use `UnitOfWork` pattern to ensure transactional consistency:

1. Load BuildingPlanApplication
2. Update foreign key
3. Update workflow status
4. Add WorkflowLog entry
5. Save all changes in one transaction

If any step fails, all changes are rolled back automatically.

## Best Practices

1. **Always create the child entity first** using the appropriate generic command handler
2. **Then orchestrate the workflow** using these workflow commands to link and update status
3. **Handle failures gracefully** - check the Result object for success/failure
4. **Provide meaningful remarks** for audit trail clarity
5. **Use appropriate assigned user IDs** for workflow tracking

## Error Handling

All workflow methods return `Result` objects:

```csharp
var result = await _mediator.Send(command);

if (!result.Succeeded)
{
    // Handle failure
    _logger.LogError("Workflow failed: {Error}", result.Message);
    return BadRequest(result.Message);
}

// Success - proceed with next steps
return Ok();
```

## Dependencies

The WorkflowOrchestrationCommandHandler requires:

- `IBuildingPlanRepository` - For loading and persisting BuildingPlanApplication
- `ICurrentUserService` - For tracking who performed the action
- `IWorkflowService` - For creating WorkflowLog entries

These are injected via dependency injection and should be registered in your IoC container.
