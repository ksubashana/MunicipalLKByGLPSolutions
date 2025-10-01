using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.Generic.Result;
using MuniLK.Application.ScheduleAppointment.Commands.CreateAppointment;
using MuniLK.Application.ScheduleAppointment.Commands.DeleteAppointment;
using MuniLK.Application.ScheduleAppointment.Commands.UpdateAppointment;
using MuniLK.Application.ScheduleAppointment.DTOs;
using MuniLK.Application.ScheduleAppointment.Queries.GetAllAppointments;
using MuniLK.Application.ScheduleAppointment.Queries.GetAppointmentById;
using MuniLK.Application.ScheduleAppointment.Queries.GetAppointmentsByUser;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace MuniLK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ScheduleAppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ScheduleAppointmentsController> _logger;

        public ScheduleAppointmentsController(IMediator mediator, ILogger<ScheduleAppointmentsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Gets all appointments for the current user
        /// </summary>
        [HttpGet("user")]
        [ProducesResponseType(typeof(IEnumerable<ScheduleAppointmentResponse>), 200)]
        public async Task<ActionResult<IEnumerable<ScheduleAppointmentResponse>>> GetUserAppointments(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var query = new GetAppointmentsByUserQuery(null, startDate, endDate);
            var appointments = await _mediator.Send(query);
            return Ok(appointments);
        }

        /// <summary>
        /// Gets all appointments in the system (admin view)
        /// </summary>
        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<ScheduleAppointmentResponse>), 200)]
        public async Task<ActionResult<IEnumerable<ScheduleAppointmentResponse>>> GetAllAppointments(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var query = new GetAllAppointmentsQuery(startDate, endDate);
            var appointments = await _mediator.Send(query);
            return Ok(appointments);
        }

        /// <summary>
        /// Gets appointments for a specific user
        /// </summary>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<ScheduleAppointmentResponse>), 200)]
        public async Task<ActionResult<IEnumerable<ScheduleAppointmentResponse>>> GetAppointmentsByUser(
            Guid userId,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var query = new GetAppointmentsByUserQuery(userId, startDate, endDate);
            var appointments = await _mediator.Send(query);
            return Ok(appointments);
        }

        /// <summary>
        /// Gets a specific appointment by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ScheduleAppointmentResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ScheduleAppointmentResponse>> GetAppointmentById(int id)
        {
            var query = new GetAppointmentByIdQuery(id);
            var appointment = await _mediator.Send(query);

            if (appointment == null)
            {
                return NotFound($"Appointment with ID {id} not found.");
            }

            return Ok(appointment);
        }

        /// <summary>
        /// Creates a new appointment
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Result<ScheduleAppointmentResponse>), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Result<ScheduleAppointmentResponse>>> CreateAppointment([FromBody] ScheduleAppointmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new CreateAppointmentCommand(request);
            var result = await _mediator.Send(command);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetAppointmentById), new { id = result.Data!.AppointmentId }, result);
        }

        /// <summary>
        /// Updates an existing appointment
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Result<ScheduleAppointmentResponse>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Result<ScheduleAppointmentResponse>>> UpdateAppointment(int id, [FromBody] UpdateScheduleAppointmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new UpdateAppointmentCommand(id, request);
            var result = await _mediator.Send(command);

            if (!result.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(result.Error) &&
                    result.Error.Contains("not found", StringComparison.OrdinalIgnoreCase))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Deletes an appointment
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Result<bool>>> DeleteAppointment(int id)
        {
            var command = new DeleteAppointmentCommand(id);
            var result = await _mediator.Send(command);

            if (!result.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(result.Error) &&
                    result.Error.Contains("not found", StringComparison.OrdinalIgnoreCase))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Syncfusion DataManager endpoint for scheduler operations
        /// </summary>
        [HttpPost("scheduler-data")]
        public async Task<object> GetSchedulerData([FromBody] DataManagerRequest dataManagerRequest)
        {
            try
            {
                // Determine if this is for all appointments or user-specific
                bool isAllAppointments = dataManagerRequest.Params?.ContainsKey("allUsers") == true &&
                                       bool.TryParse(dataManagerRequest.Params["allUsers"]?.ToString(), out bool showAll) && showAll;

                IEnumerable<ScheduleAppointmentResponse> appointments;

                if (isAllAppointments)
                {
                    var allQuery = new GetAllAppointmentsQuery();
                    appointments = await _mediator.Send(allQuery);
                }
                else
                {
                    var userQuery = new GetAppointmentsByUserQuery();
                    appointments = await _mediator.Send(userQuery);
                }

                // Convert to format expected by Syncfusion Scheduler
                var schedulerData = appointments.Select(a => new
                {
                    Id = a.AppointmentId,
                    Subject = a.Subject,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Location = a.Location,
                    Description = a.Description,
                    IsAllDay = a.AllDay,
                    IsReadonly = a.IsReadOnly,
                    RecurrenceRule = a.RecurrenceRule,
                    RecurrenceException = a.RecurrenceExDate,
                    RecurrenceID = a.RecurrenceID,
                    StartTimezone = a.StartTimeZone,
                    EndTimezone = a.EndTimeZone,
                    CategoryColor = a.CustomStyle,
                    OwnerId = a.OwnerId,
                    OwnerName = a.OwnerName,
                    OwnerRole = a.OwnerRole
                });

                return new { result = schedulerData, count = schedulerData.Count() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving scheduler data");
                return BadRequest("Error retrieving scheduler data");
            }
        }

        /// <summary>
        /// Syncfusion DataManager endpoint for CRUD operations
        /// </summary>
        [HttpPost("scheduler-crud")]
        public async Task<object> SchedulerCrud([FromBody] CRUDModel<ScheduleAppointmentRequest> crudModel)
        {
            try
            {
                if (crudModel.Action == "insert" && crudModel.Value != null)
                {
                    var createCommand = new CreateAppointmentCommand(crudModel.Value);
                    var createResult = await _mediator.Send(createCommand);
                    return new { result = createResult.Data };
                }
                else if (crudModel.Action == "update" && crudModel.Value != null && crudModel.Key != null)
                {
                    var updateRequest = new UpdateScheduleAppointmentRequest
                    {
                        Subject = crudModel.Value.Subject,
                        Location = crudModel.Value.Location,
                        Description = crudModel.Value.Description,
                        StartTime = crudModel.Value.StartTime,
                        EndTime = crudModel.Value.EndTime,
                        StartTimeZone = crudModel.Value.StartTimeZone,
                        EndTimeZone = crudModel.Value.EndTimeZone,
                        AllDay = crudModel.Value.AllDay,
                        Recurrence = crudModel.Value.Recurrence,
                        RecurrenceRule = crudModel.Value.RecurrenceRule,
                        RecurrenceExDate = crudModel.Value.RecurrenceExDate,
                        RecurrenceID = crudModel.Value.RecurrenceID,
                        FollowingID = crudModel.Value.FollowingID,
                        IsBlock = crudModel.Value.IsBlock,
                        IsReadOnly = crudModel.Value.IsReadOnly,
                        Department = crudModel.Value.Department,
                        OwnerId = crudModel.Value.OwnerId,
                        OwnerRole = crudModel.Value.OwnerRole,
                        Priority = crudModel.Value.Priority,
                        Reminder = crudModel.Value.Reminder,
                        CustomStyle = crudModel.Value.CustomStyle,
                        AppointmentGroup = crudModel.Value.AppointmentGroup
                    };

                    var updateCommand = new UpdateAppointmentCommand((int)crudModel.Key, updateRequest);
                    var updateResult = await _mediator.Send(updateCommand);
                    return new { result = updateResult.Data };
                }
                else if (crudModel.Action == "remove" && crudModel.Key != null)
                {
                    var deleteCommand = new DeleteAppointmentCommand((int)crudModel.Key);
                    var deleteResult = await _mediator.Send(deleteCommand);
                    return new { result = deleteResult.Data };
                }

                return BadRequest("Invalid CRUD operation");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing scheduler CRUD operation");
                return BadRequest("Error performing operation");
            }
        }
    }
}