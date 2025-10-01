using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.PropertiesLK.DTOs
{
    public class PropertyResponse
    {
        public Guid? Id { get; set; }
        public string PropertyId { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? TitleDeedNumber { get; set; }
        public decimal AssessmentValue { get; set; }
        public bool IsCommercialUse { get; set; }
        // Add more fields as needed
    }

}
