using MediatR;
using MuniLK.Application.Generic.Result;
using MuniLK.Application.ScheduleAppointment.DTOs;

namespace MuniLK.Application.ScheduleAppointment.Commands.UpdateAppointment
{
    public record UpdateAppointmentCommand(int AppointmentId, UpdateScheduleAppointmentRequest Request) : IRequest<Result<ScheduleAppointmentResponse>>;
}