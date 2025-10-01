// Inside your Worker project (e.g., in a new folder like 'WorkerServices' or 'Services')
// File: MuniLK.Worker.Services/WorkerCurrentTenantService.cs
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Domain.Constants;
namespace MuniLK.Worker.Services
{
    public class WorkerCurrentTenantService : ICurrentTenantService
    {
        private Guid? _currentTenantId; // This will hold the tenant ID for the current message/task

        // Constructor for setting a default or from configuration if needed
        public WorkerCurrentTenantService(IConfiguration configuration)
        {
            var defaultTenantId = configuration["Worker:DefaultTenantId"] ?? SystemConstants.SystemTenantId.ToString();
            _currentTenantId = Guid.TryParse(defaultTenantId, out var tenantId) ? tenantId : Guid.Empty;
        }

        public Guid? GetTenantId()
        {
            return _currentTenantId;
        }

        // IMPORTANT: This setter is how your worker will pass the tenant ID from the message
        public void SetCurrentTenantId(Guid? tenantId)
        {
            _currentTenantId = tenantId;
        }

        public void SetTenantId(Guid tenantId)
        {
            _currentTenantId = tenantId;
        }
    }
}