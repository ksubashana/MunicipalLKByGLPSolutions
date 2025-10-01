using MediatR;
using MuniLK.Application.Assignments;
using MuniLK.Application.Assignments.Commands;
using MuniLK.Application.Assignments.DTOs;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Applications.Interfaces;
using MuniLK.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.Assignments.Commands
{
    public class CreateAssignmentCommandHandler : IRequestHandler<CreateAssignmentCommand, Guid>
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly ICurrentTenantService _currentTenantService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IContactRepository _contactRepository;
        private readonly IEmailService _emailService;

        public CreateAssignmentCommandHandler(
            IAssignmentRepository assignmentRepository, 
            ICurrentTenantService currentTenantService, 
            ICurrentUserService currentUserService,
            IContactRepository contactRepository,
            IEmailService emailService)
        {
            _assignmentRepository = assignmentRepository;
            _currentTenantService = currentTenantService;
            _currentUserService = currentUserService;
            _contactRepository = contactRepository;
            _emailService = emailService;
        }

        public async Task<Guid> Handle(CreateAssignmentCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var assignment = new Assignment
            {
                Id = Guid.NewGuid(),
                AssignedBy = Guid.Parse(_currentUserService.UserId),
                AssignedTo = request.AssignedToUserId,
                TenantId = _currentTenantService.GetTenantId(),
                AssignmentDate = request.AssignmentDate,
                TaskType = request.TaskType,
                Notes = request.Notes,
                EntityId = request.EntityId,
                EntityType = request.EntityType,
                ModuleId = request.ModuleId,
                CreatedAt = DateTime.UtcNow,
                FeatureId = request.FeatureId
            };

            await _assignmentRepository.AddAsync(assignment);
            await _assignmentRepository.SaveChangesAsync();

            // Send email notification to the assigned inspector
            try
            {
                // Get the contact information for the assigned inspector
                var inspectorContact = await _contactRepository.GetByIdAsync(request.AssignedToUserId);
                if (inspectorContact != null && !string.IsNullOrWhiteSpace(inspectorContact.Email))
                {
                    await _emailService.SendInspectionAssignmentEmailAsync(
                        inspectorContact.Email,
                        inspectorContact.FullName,
                        request.FeatureId ?? "N/A",
                        request.AssignmentDate,
                        request.Notes);
                }
            }
            catch
            {
                // Log error but don't fail the assignment creation if email fails
                // Email service already handles logging
            }

            return assignment.Id;
        }
    }
}
