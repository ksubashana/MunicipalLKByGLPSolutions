using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Domain.Entities
{
    public class Report
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; }
        public string BlobPath { get; set; } = default!;
        public Guid? TenantId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public Guid ModuleId { get; set; } 
        public Module Module { get; set; } 
    }
}
