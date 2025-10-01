using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.FeatureId.DTOs
{
    public class GenerateFeatureIdRequest
    {
        public string ConfigKey { get; set; } = string.Empty; // e.g., "PermitFeatureID"
    }

}
