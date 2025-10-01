using MediatR;
using MuniLK.Application.ScheduleAppointment.DTOs;

namespace MuniLK.Application.ScheduleAppointment.Queries.GetAppointmentById
{
    public record GetAppointmentByIdQuery(int AppointmentId) : IRequest<ScheduleAppointmentResponse?>;
}