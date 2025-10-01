using MediatR;
using MuniLK.Application.ScheduleAppointment.DTOs;
using MuniLK.Application.ScheduleAppointment.Interfaces;
using MuniLK.Application.ScheduleAppointment.Mappings;

namespace MuniLK.Application.ScheduleAppointment.Queries.GetAllAppointments
{
    public class GetAllAppointmentsQueryHandler : IRequestHandler<GetAllAppointmentsQuery, IEnumerable<ScheduleAppointmentResponse>>
    {
        private readonly IScheduleAppointmentRepository _repository;

        public GetAllAppointmentsQueryHandler(IScheduleAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ScheduleAppointmentResponse>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Domain.Entities.ScheduleAppointments> appointments;

            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                appointments = await _repository.GetByDateRangeAsync(request.StartDate.Value, request.EndDate.Value);
            }
            else
            {
                appointments = await _repository.GetAllAsync();
            }

            return appointments.Select(a => a.ToResponse());
        }
    }
}