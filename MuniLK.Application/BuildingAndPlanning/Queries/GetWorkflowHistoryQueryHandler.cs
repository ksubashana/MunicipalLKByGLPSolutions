using MediatR;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.BuildingAndPlanning.Queries;
using MuniLK.Application.Generic.Result;
using MuniLK.Applications.Interfaces; // IContactRepository

namespace MuniLK.Application.BuildingAndPlanning.Queries
{
    /// <summary>
    /// Handler for retrieving workflow history for building plan applications.
    /// Resolves performer display name via Contact repository (preferred over user repo per requirement).
    /// </summary>
    public class GetWorkflowHistoryQueryHandler : IRequestHandler<GetWorkflowHistoryQuery, Result<List<WorkflowHistoryResponse>>>
    {
        private readonly IBuildingPlanRepository _repository;
        private readonly IContactRepository _contactRepository;

        public GetWorkflowHistoryQueryHandler(IBuildingPlanRepository repository, IContactRepository contactRepository)
        {
            _repository = repository;
            _contactRepository = contactRepository;
        }

        public async Task<Result<List<WorkflowHistoryResponse>>> Handle(GetWorkflowHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Verify the application exists
                var application = await _repository.GetByIdAsync(request.ApplicationId, cancellationToken);
                if (application == null)
                {
                    return Result<List<WorkflowHistoryResponse>>.Failure("Building plan application not found");
                }

                // Retrieve workflow logs
                var workflowLogs = await _repository.GetWorkflowLogsAsync(request.ApplicationId, cancellationToken);

                var history = new List<WorkflowHistoryResponse>(workflowLogs.Count);
                foreach (var w in workflowLogs.OrderByDescending(x => x.PerformedAt))
                {
                    string? displayName = null;
                    if (Guid.TryParse(w.PerformedByUserId, out var contactId))
                    {
                        var contact = await _contactRepository.GetByIdAsync(contactId);
                        if (contact != null)
                        {
                            displayName = contact.FullName ?? contact.NIC ?? contact.Email ?? contact.PhoneNumber;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(displayName))
                        displayName = w.IsSystemGenerated ? "System" : w.PerformedByUserId;

                    history.Add(new WorkflowHistoryResponse
                    {
                        Id = w.Id,
                        ActionTaken = w.ActionTaken,
                        PreviousStatus = w.PreviousStatus,
                        NewStatus = w.NewStatus,
                        Remarks = w.Remarks,
                        PerformedByUserId = w.PerformedByUserId,
                        PerformedByDisplayName = displayName,
                        PerformedByRole = w.PerformedByRole,
                        AssignedToUserId = w.AssignedToUserId,
                        IsSystemGenerated = w.IsSystemGenerated,
                        PerformedAt = w.PerformedAt
                    });
                }

                return Result<List<WorkflowHistoryResponse>>.Success(history);
            }
            catch (Exception ex)
            {
                return Result<List<WorkflowHistoryResponse>>.Failure($"Error retrieving workflow history: {ex.Message}");
            }
        }
    }
}