using MediatR;
using MuniLK.Application.Generic.Result;

namespace MuniLK.Application.ScheduleAppointment.Commands.DeleteAppointment
{
    public record DeleteAppointmentCommand(int AppointmentId) : IRequest<Result<bool>>;
}