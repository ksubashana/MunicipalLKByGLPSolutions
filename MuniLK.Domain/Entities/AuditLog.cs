using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; }

        public Guid TenantId { get; set; }

        public string TableName { get; set; } = default!;
        public string ColumnName { get; set; } = default!;
        public string EntityId { get; set; } = default!;

        public string OldValue { get; set; } = default!;
        public string NewValue { get; set; } = default!;

        public string ChangedByUserId { get; set; } = default!;
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }


}
