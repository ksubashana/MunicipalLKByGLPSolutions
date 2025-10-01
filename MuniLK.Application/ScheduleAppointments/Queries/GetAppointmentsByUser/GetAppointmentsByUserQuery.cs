using MediatR;
using MuniLK.Application.ScheduleAppointment.DTOs;

namespace MuniLK.Application.ScheduleAppointment.Queries.GetAppointmentsByUser
{
    public record GetAppointmentsByUserQuery(Guid? UserId = null, DateTime? StartDate = null, DateTime? EndDate = null) : IRequest<IEnumerable<ScheduleAppointmentResponse>>;
}