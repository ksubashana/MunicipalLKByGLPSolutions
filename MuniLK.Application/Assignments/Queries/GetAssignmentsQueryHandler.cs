using MediatR;
using MuniLK.Application.Assignments.DTOs;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Domain.Entities;
using MuniLK.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.Assignments.Queries
{
    public class GetAssignmentsQueryHandler
        : IRequestHandler<GetAssignmentsQuery, List<AssignmentResponse>>
    {
        private readonly IAssignmentRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentTenantService _currentTenantService;

        public GetAssignmentsQueryHandler(IAssignmentRepository repository, IUserRepository userRepository, ICurrentTenantService currentTenantService)
        {
            _repository = repository;
            _userRepository = userRepository;
            _currentTenantService = currentTenantService;
        }

        public async Task<List<AssignmentResponse>> Handle(GetAssignmentsQuery request, CancellationToken cancellationToken)
        {
            // Fetch assignments filtered by Module, Entity, and Tenant
            var assignments = await _repository.GetByModuleAndEntityAsync(
                request.ModuleId, request.EntityId, _currentTenantService.GetTenantId());

            var responses = new List<AssignmentResponse>();

            foreach (var assignment in assignments)
            {
                User? assignee = null;
                User? assigner = null;

                if (assignment.AssignedTo != Guid.Empty)
                {
                    assignee = await _userRepository.GetUserByIdAsync(assignment.AssignedTo);
                }

                if (assignment.AssignedBy.HasValue && assignment.AssignedBy.Value != Guid.Empty)
                {
                    assigner = await _userRepository.GetUserByIdAsync(assignment.AssignedBy.Value);
                }

                responses.Add(new AssignmentResponse
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
                    FeatureId = assignment.FeatureId
                });
            }

            return responses;
        }
    }
}
