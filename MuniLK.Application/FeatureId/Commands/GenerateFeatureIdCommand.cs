using MediatR;
using MuniLK.Application.FeatureId.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.FeatureId.Commands
{
    public class GenerateFeatureIdCommand : IRequest<FeatureIdResponse>
    {
        public string ConfigKey { get; set; } = string.Empty;
    }
}
