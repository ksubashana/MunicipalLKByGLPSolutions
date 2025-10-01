using MuniLK.Application.Generic.Interfaces;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;

namespace MuniLK.Web.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly MuniLKDbContext _context;
        private readonly ICurrentTenantService _currentTenantService;

        public WorkflowService(MuniLKDbContext context, ICurrentTenantService currentTenantService)
        {
            _context = context;
            _currentTenantService = currentTenantService;
        }

        public async Task AddLogAsync(
            Guid applicationId,
            string actionTaken,
            string? previousStatus,
            string newStatus,
            string? remarks,
            string performedByUserId,
            string? performedByRole,
            string? assignedToUserId,
            bool isSystemGenerated,
            CancellationToken ct)
        {
            var tenantId = _currentTenantService.GetTenantId()
                           ?? throw new Exception("Tenant not found.");

            var log = new WorkflowLog
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                ApplicationId = applicationId,
                ActionTaken = actionTaken,
                PreviousStatus = previousStatus,
                NewStatus = newStatus,
                Remarks = remarks,
                PerformedByUserId = performedByUserId,
                PerformedByRole = performedByRole,
                AssignedToUserId = assignedToUserId,
                IsSystemGenerated = isSystemGenerated,
                PerformedAt = DateTime.UtcNow
            };

            _context.WorkflowLogs.Add(log);
            // ❌ Do NOT SaveChangesAsync here — let Unit of Work commit it
            await Task.CompletedTask;
        }
    }

}
