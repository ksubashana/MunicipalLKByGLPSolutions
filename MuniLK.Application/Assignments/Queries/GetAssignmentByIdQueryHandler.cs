using MediatR;
using MuniLK.Application.Assignments.DTOs;
using MuniLK.Application.Assignments;
using MuniLK.Application.Assignments.Queries;
using MuniLK.Domain.Entities;
using MuniLK.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.Assignments.Queries
{
    public class GetAssignmentByIdQueryHandler : IRequestHandler<GetAssignmentByIdQuery, AssignmentResponse?>
    {
        private readonly IAssignmentRepository _repository;
        private readonly IUserRepository _userRepository;

        public GetAssignmentByIdQueryHandler(IAssignmentRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<AssignmentResponse?> Handle(GetAssignmentByIdQuery request, CancellationToken cancellationToken)
        {
            var assignment = await _repository.GetByIdAsync(request.AssignmentId);
            if (assignment == null) return null;

            User? assignee = null;
            User? assigner = null;
            if (assignment.AssignedTo != Guid.Empty)
                assignee = await _userRepository.GetUserByIdAsync(assignment.AssignedTo);
            if (assignment.AssignedBy.HasValue && assignment.AssignedBy.Value != Guid.Empty)
                assigner = await _userRepository.GetUserByIdAsync(assignment.AssignedBy.Value);

            return new AssignmentResponse
            {
                Id = assignment.Id,
                AssignedToUserId = assignment.AssignedTo,
                AssignedToName = assignee?.Username ?? string.Empty,
                AssignedByUserId = assignment.AssignedBy,
                AssignedByName = assigner?.Username ?? string.Empty,
                EntityId = assignment.EntityId,
                EntityType = assignment.EntityType,
                ModuleId = assignment.ModuleId,
                AssignmentDate = assignment.AssignmentDate,
                DueDate = assignment.DueDate,
                TaskType = assignment.TaskType,
                Notes = assignment.Notes ?? string.Empty,
                IsCompleted = assignment.IsCompleted,
                CompletedAt = assignment.CompletedAt,
                Outcome = assignment.Outcome,
                OutcomeRemarks = assignment.OutcomeRemarks,
                FeatureId = assignment.FeatureId,
                ModuleName = string.Empty,
                EntityReference = string.Empty
            };
        }
    }
}
