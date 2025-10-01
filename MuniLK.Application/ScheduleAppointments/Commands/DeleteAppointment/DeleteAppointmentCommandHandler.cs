using MediatR;
using MuniLK.Application.Generic.Result;
using MuniLK.Application.ScheduleAppointment.Interfaces;
using MuniLK.Domain.Interfaces;

namespace MuniLK.Application.ScheduleAppointment.Commands.DeleteAppointment
{
    public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, Result<bool>>
    {
        private readonly IScheduleAppointmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAppointmentCommandHandler(
            IScheduleAppointmentRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var exists = await _repository.ExistsAsync(request.AppointmentId);
                if (!exists)
                {
                    return Result<bool>.Failure($"Appointment with ID {request.AppointmentId} not found.");
                }

                await _repository.DeleteAsync(request.AppointmentId);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to delete appointment: {ex.Message}");
            }
        }
    }
}