using MediatR;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.Generic.Result;
using MuniLK.Application.ScheduleAppointment.DTOs;
using MuniLK.Application.ScheduleAppointment.Interfaces;
using MuniLK.Application.ScheduleAppointment.Mappings;
using MuniLK.Domain.Interfaces;

namespace MuniLK.Application.ScheduleAppointment.Commands.CreateAppointment
{
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Result<ScheduleAppointmentResponse>>
    {
        private readonly IScheduleAppointmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICurrentTenantService _currentTenantService;

        public CreateAppointmentCommandHandler(
            IScheduleAppointmentRepository repository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            ICurrentTenantService currentTenantService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _currentTenantService = currentTenantService;
        }

        public async Task<Result<ScheduleAppointmentResponse>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tenantId = _currentTenantService.GetTenantId();
                var currentUser = _currentUserService.UserName;

                // If OwnerId is not specified, assign to current user
                if (!request.Request.OwnerId.HasValue && !string.IsNullOrEmpty(_currentUserService.UserId))
                {
                    if (Guid.TryParse(_currentUserService.UserId, out var userId))
                    {
                        request.Request.OwnerId = userId;
                    }
                }

                var entity = request.Request.ToEntity(tenantId, currentUser);

                await _repository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var response = entity.ToResponse();

                return Result<ScheduleAppointmentResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<ScheduleAppointmentResponse>.Failure($"Failed to create appointment: {ex.Message}");
            }
        }
    }
}