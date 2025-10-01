using MediatR;
using MuniLK.Application.ScheduleAppointment.DTOs;

namespace MuniLK.Application.ScheduleAppointment.Queries.GetAllAppointments
{
    public record GetAllAppointmentsQuery(DateTime? StartDate = null, DateTime? EndDate = null) : IRequest<IEnumerable<ScheduleAppointmentResponse>>;
}