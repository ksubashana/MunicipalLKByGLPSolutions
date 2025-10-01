using MuniLK.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Domain.Entities
{
    public class FeatureIdAudit : IHasTenant
    {
        public int Id { get; set; }
        public Guid? TenantId { get; set; }
        public string ConfigKey { get; set; } = string.Empty;
        public int SequenceNumber { get; set; }
        public string Year { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
    }

}
