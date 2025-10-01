using MediatR;
using MuniLK.Application.ScheduleAppointment.DTOs;
using MuniLK.Application.ScheduleAppointment.Interfaces;
using MuniLK.Application.ScheduleAppointment.Mappings;

namespace MuniLK.Application.ScheduleAppointment.Queries.GetAppointmentById
{
    public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, ScheduleAppointmentResponse?>
    {
        private readonly IScheduleAppointmentRepository _repository;

        public GetAppointmentByIdQueryHandler(IScheduleAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<ScheduleAppointmentResponse?> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
        {
            var appointment = await _repository.GetByIdAsync(request.AppointmentId);
            return appointment?.ToResponse();
        }
    }
}