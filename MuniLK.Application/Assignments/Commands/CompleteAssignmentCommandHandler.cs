using MediatR;
using MuniLK.Application.Assignments;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.Assignments.Commands
{
    public class CompleteAssignmentCommandHandler : IRequestHandler<CompleteAssignmentCommand, bool>
    {
        private readonly IAssignmentRepository _repo;

        public CompleteAssignmentCommandHandler(IAssignmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(CompleteAssignmentCommand command, CancellationToken ct)
        {
            var req = command.Request;
            var assignment = await _repo.GetByIdAsync(req.AssignmentId);
            if (assignment is null) return false;

            assignment.IsCompleted = true;
            assignment.CompletedAt = req.CompletedAt ?? DateTime.UtcNow;
            assignment.Outcome = req.Outcome;
            assignment.OutcomeRemarks = req.Remarks;
            assignment.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(assignment);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}