using MediatR;
using MuniLK.Application.Generic.Result;
using MuniLK.Application.ScheduleAppointment.DTOs;

namespace MuniLK.Application.ScheduleAppointment.Commands.CreateAppointment
{
    public record CreateAppointmentCommand(ScheduleAppointmentRequest Request) : IRequest<Result<ScheduleAppointmentResponse>>;
}