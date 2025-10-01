using MediatR;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.ScheduleAppointment.DTOs;
using MuniLK.Application.ScheduleAppointment.Interfaces;
using MuniLK.Application.ScheduleAppointment.Mappings;

namespace MuniLK.Application.ScheduleAppointment.Queries.GetAppointmentsByUser
{
    public class GetAppointmentsByUserQueryHandler : IRequestHandler<GetAppointmentsByUserQuery, IEnumerable<ScheduleAppointmentResponse>>
    {
        private readonly IScheduleAppointmentRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public GetAppointmentsByUserQueryHandler(
            IScheduleAppointmentRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<ScheduleAppointmentResponse>> Handle(GetAppointmentsByUserQuery request, CancellationToken cancellationToken)
        {
            // If no UserId provided, use current user
            var userId = request.UserId;
            if (!userId.HasValue && !string.IsNullOrEmpty(_currentUserService.UserId))
            {
                if (Guid.TryParse(_currentUserService.UserId, out var currentUserId))
                {
                    userId = currentUserId;
                }
            }

            if (!userId.HasValue)
            {
                return new List<ScheduleAppointmentResponse>();
            }

            IEnumerable<Domain.Entities.ScheduleAppointments> appointments;

            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                appointments = await _repository.GetByUserAndDateRangeAsync(userId.Value, request.StartDate.Value, request.EndDate.Value);
            }
            else
            {
                appointments = await _repository.GetByUserIdAsync(userId.Value);
            }

            return appointments.Select(a => a.ToResponse());
        }
    }
}