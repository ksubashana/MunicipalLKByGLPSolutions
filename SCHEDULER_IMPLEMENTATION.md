# Syncfusion Blazor Scheduler Implementation

## Overview
This document describes the newly implemented Syncfusion Blazor Scheduler feature for the municipal management system. The implementation follows Clean Architecture principles with CQRS pattern using MediatR.

## Features Implemented

### 1. Core Functionality
- ✅ Full CRUD operations (Create, Read, Update, Delete) for appointments
- ✅ User-specific scheduler view (shows only current user's appointments)
- ✅ All users scheduler view (shows all appointments in the system)
- ✅ Tenant-aware data isolation
- ✅ Role-based access and display

### 2. UI Features
- ✅ Modern Syncfusion Scheduler component integration
- ✅ Multiple view modes (Day, Week, Work Week, Month, Agenda)
- ✅ Toggle between personal and all users view
- ✅ User context display (name and assigned role)
- ✅ Custom event templates showing location, owner, and role information
- ✅ Responsive design with Bootstrap integration

### 3. Technical Features
- ✅ Clean Architecture implementation
- ✅ CQRS with MediatR
- ✅ Repository pattern
- ✅ Entity Framework Core integration
- ✅ Comprehensive validation
- ✅ Exception handling
- ✅ Audit trails (CreatedBy, UpdatedBy, timestamps)

## Architecture Components

### Domain Layer
- **Entity**: `ScheduleAppointments` - Enhanced with tenant support and proper relationships
- **Interfaces**: `IHasTenant` implementation for multi-tenancy

### Application Layer
- **DTOs**: 
  - `ScheduleAppointmentRequest` - For creating appointments
  - `ScheduleAppointmentResponse` - For returning appointment data
  - `UpdateScheduleAppointmentRequest` - For updating appointments
- **Commands**: `CreateAppointmentCommand`, `UpdateAppointmentCommand`, `DeleteAppointmentCommand`
- **Queries**: `GetAppointmentsByUserQuery`, `GetAllAppointmentsQuery`, `GetAppointmentByIdQuery`
- **Mappings**: Extension methods for entity/DTO conversion
- **Interfaces**: `IScheduleAppointmentRepository`

### Infrastructure Layer
- **Repository**: `ScheduleAppointmentRepository` - Full async implementation
- **Configuration**: `ScheduleAppointmentsConfiguration` - EF Core entity configuration
- **DbContext**: Integration with `MuniLKDbContext`

### API Layer
- **Controller**: `ScheduleAppointmentsController` - RESTful endpoints with Syncfusion DataManager support
- **Endpoints**:
  - `GET /api/scheduleappointments/user` - Get current user's appointments
  - `GET /api/scheduleappointments/all` - Get all appointments
  - `GET /api/scheduleappointments/user/{userId}` - Get specific user's appointments
  - `GET /api/scheduleappointments/{id}` - Get appointment by ID
  - `POST /api/scheduleappointments` - Create appointment
  - `PUT /api/scheduleappointments/{id}` - Update appointment
  - `DELETE /api/scheduleappointments/{id}` - Delete appointment
  - `POST /api/scheduleappointments/scheduler-data` - Syncfusion DataManager endpoint
  - `POST /api/scheduleappointments/scheduler-crud` - Syncfusion CRUD operations

### Web Layer
- **Page**: `/scheduler` - Main scheduler interface
- **Navigation**: Integrated into left sidebar with calendar icon

## Database Schema

The `ScheduleAppointments` table includes:

```sql
- AppointmentId (int, PK, Identity)
- TenantId (uniqueidentifier, nullable) -- Multi-tenancy support
- Subject (nvarchar(200), required) -- Appointment title
- Location (nvarchar(400), optional) -- Appointment location
- Description (nvarchar(max), optional) -- Detailed description
- StartTime (datetime2, required) -- Start date/time
- EndTime (datetime2, required) -- End date/time
- StartTimeZone (nvarchar(100)) -- Start timezone
- EndTimeZone (nvarchar(100)) -- End timezone
- AllDay (bit) -- All-day event flag
- Recurrence (bit) -- Recurring event flag
- RecurrenceRule (nvarchar(max)) -- Recurrence pattern
- RecurrenceExDate (nvarchar(max)) -- Recurrence exceptions
- RecurrenceID (int, nullable) -- Parent recurrence ID
- FollowingID (int, nullable) -- Following recurrence ID
- IsBlock (bit) -- Block time flag
- IsReadOnly (bit) -- Read-only flag
- Department (int, nullable) -- Department ID
- OwnerId (uniqueidentifier, nullable) -- Owner user ID
- OwnerRole (nvarchar(100)) -- Owner role for display
- Priority (int) -- Priority level
- Reminder (int) -- Reminder settings
- CustomStyle (nvarchar(100)) -- Custom styling
- CommonGuid (nvarchar(100)) -- Common GUID
- AppTaskId (nvarchar(100)) -- Application task ID
- Guid (nvarchar(100)) -- Entity GUID
- AppointmentGroup (nvarchar(100)) -- Appointment grouping
- CreatedBy (nvarchar(100)) -- Audit: Created by
- CreatedDate (datetime2) -- Audit: Created date
- UpdatedBy (nvarchar(100)) -- Audit: Updated by
- UpdatedDate (datetime2, nullable) -- Audit: Updated date
```

## Setup Instructions

### 1. Prerequisites
- .NET 9.0 SDK
- Entity Framework Core
- Syncfusion Blazor license (for production)

### 2. Database Migration
Run the following commands to create the database migration:

```bash
# From the solution root
dotnet ef migrations add AddScheduleAppointments --project MuniLK.Infrastructure --startup-project MuniLK.API
dotnet ef database update --project MuniLK.Infrastructure --startup-project MuniLK.API
```

### 3. Service Registration
The services are already registered in `MuniLK.API/Program.cs`:
- Repository registration
- Required using statements

### 4. Navigation Access
Navigate to `/scheduler` in the web application or use the "Scheduler" link in the left sidebar.

## Usage

### Personal Scheduler Mode
- Shows only appointments assigned to the logged-in user
- User can create, edit, and delete their own appointments
- Displays user name and role in the header

### All Users Mode
- Shows appointments for all users in the system (admin view)
- Event templates display owner information
- Useful for managers and administrators

### Creating Appointments
1. Click on any time slot in the scheduler
2. Fill in the appointment details in the popup
3. Assign to a user (defaults to current user)
4. Save the appointment

### Editing Appointments
1. Click on an existing appointment
2. Modify the details in the popup
3. Save changes

### Deleting Appointments
1. Click on an appointment
2. Use the delete option in the popup
3. Confirm deletion

## Testing

### Manual Testing
1. Start the API and Web projects
2. Navigate to `/scheduler`
3. Test creating, editing, and deleting appointments
4. Toggle between personal and all users views
5. Verify user context display
6. Test different calendar views (Day, Week, Month, etc.)

### API Testing
Use tools like Postman or Swagger UI to test the API endpoints:
- Create appointments via POST
- Retrieve appointments via GET
- Update appointments via PUT
- Delete appointments via DELETE

## Troubleshooting

### Common Issues
1. **Missing Database Table**: Run the EF migrations
2. **Syncfusion License**: Ensure proper license for production use
3. **Authentication**: Verify user is authenticated to access scheduler
4. **API Errors**: Check controller registration and dependency injection

### Logging
The implementation includes comprehensive logging for debugging:
- Controller actions
- Repository operations
- Command/Query handlers

## Future Enhancements

Potential improvements:
- Email notifications for appointments
- Calendar synchronization (Google Calendar, Outlook)
- Department-based filtering
- Advanced recurrence patterns
- Appointment templates
- Resource booking (rooms, equipment)
- Conflict detection
- Drag-and-drop scheduling optimization

## Support

For issues or questions:
1. Check the application logs
2. Verify database connectivity
3. Ensure all services are properly registered
4. Review Syncfusion documentation for component-specific issues