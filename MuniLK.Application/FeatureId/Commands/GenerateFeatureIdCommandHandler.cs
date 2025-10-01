using MediatR;
using MuniLK.Application.FeatureId.DTOs;
using MuniLK.Application.FeatureId.Interfaces;
using MuniLK.Application.Generic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.FeatureId.Commands
{
    public class GenerateFeatureIdCommandHandler : IRequestHandler<GenerateFeatureIdCommand, FeatureIdResponse>
    {
        private readonly IFeatureIdService _featureIdService;

        public GenerateFeatureIdCommandHandler(IFeatureIdService featureIdService)
        {
            _featureIdService = featureIdService;
        }

        public async Task<FeatureIdResponse> Handle(GenerateFeatureIdCommand request, CancellationToken cancellationToken)
        {
            var id = await _featureIdService.GenerateFeatureIdAsync(request.ConfigKey, cancellationToken);
            return new FeatureIdResponse { GeneratedId = id };
        }
    }
}
