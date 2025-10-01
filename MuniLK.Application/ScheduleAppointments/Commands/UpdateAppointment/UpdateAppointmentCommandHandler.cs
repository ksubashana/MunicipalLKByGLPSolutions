using MediatR;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.Generic.Result;
using MuniLK.Application.ScheduleAppointment.DTOs;
using MuniLK.Application.ScheduleAppointment.Interfaces;
using MuniLK.Application.ScheduleAppointment.Mappings;
using MuniLK.Domain.Interfaces;

namespace MuniLK.Application.ScheduleAppointment.Commands.UpdateAppointment
{
    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, Result<ScheduleAppointmentResponse>>
    {
        private readonly IScheduleAppointmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateAppointmentCommandHandler(
            IScheduleAppointmentRepository repository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<ScheduleAppointmentResponse>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(request.AppointmentId);
                if (entity == null)
                {
                    return Result<ScheduleAppointmentResponse>.Failure($"Appointment with ID {request.AppointmentId} not found.");
                }

                var currentUser = _currentUserService.UserName;
                entity.UpdateFromDto(request.Request, currentUser);

                await _repository.UpdateAsync(entity);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var response = entity.ToResponse();

                return Result<ScheduleAppointmentResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<ScheduleAppointmentResponse>.Failure($"Failed to update appointment: {ex.Message}");
            }
        }
    }
}