using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.FeatureId.Interfaces
{
    public interface IFeatureIdService
    {
        Task<string> GenerateFeatureIdAsync(string configKey, CancellationToken cancellationToken);
    }

}
