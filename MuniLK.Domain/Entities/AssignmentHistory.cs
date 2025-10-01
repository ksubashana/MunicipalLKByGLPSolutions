using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Domain.Entities
{
    public class AssignmentHistory
    {
        public Guid Id { get; set; }

        public Guid TenantId { get; set; }

        public Guid ApplicationId { get; set; }

        public string FromUserId { get; set; } = default!;
        public string ToUserId { get; set; } = default!;

        public string? Remarks { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }

}
