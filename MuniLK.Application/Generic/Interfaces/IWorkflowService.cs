using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Generic.Interfaces
{
    public interface IWorkflowService
    {
        Task AddLogAsync(
            Guid applicationId,
            string actionTaken,
            string? previousStatus,
            string newStatus,
            string? remarks,
            string performedByUserId,
            string? performedByRole,
            string? assignedToUserId,
            bool isSystemGenerated,
            CancellationToken ct);
    }

}
