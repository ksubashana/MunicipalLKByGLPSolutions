using MediatR;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.BuildingAndPlanning.Queries;
using MuniLK.Application.Generic.Result;

namespace MuniLK.Application.BuildingAndPlanning.Queries
{
    /// <summary>
    /// Handler for retrieving workflow history for building plan applications
    /// </summary>
    public class GetWorkflowHistoryQueryHandler : IRequestHandler<GetWorkflowHistoryQuery, Result<List<WorkflowHistoryResponse>>>
    {
        private readonly IBuildingPlanRepository _repository;

        public GetWorkflowHistoryQueryHandler(IBuildingPlanRepository repository)
        {
            _repository = repository;
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

                var workflowHistory = workflowLogs.Select(w => new WorkflowHistoryResponse
                {
                    Id = w.Id,
                    ActionTaken = w.ActionTaken,
                    PreviousStatus = w.PreviousStatus,
                    NewStatus = w.NewStatus,
                    Remarks = w.Remarks,
                    PerformedByUserId = w.PerformedByUserId,
                    PerformedByRole = w.PerformedByRole,
                    AssignedToUserId = w.AssignedToUserId,
                    IsSystemGenerated = w.IsSystemGenerated,
                    PerformedAt = w.PerformedAt
                }).ToList();

                return Result<List<WorkflowHistoryResponse>>.Success(workflowHistory);
            }
            catch (Exception ex)
            {
                return Result<List<WorkflowHistoryResponse>>.Failure($"Error retrieving workflow history: {ex.Message}");
            }
        }
    }
}