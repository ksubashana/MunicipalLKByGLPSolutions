using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.Logging;

namespace MuniLK.Web.Services
{
    /// <summary>
    /// Global circuit handler to monitor Blazor Server circuit lifecycle events.
    /// Logs circuit open/close and connection up/down events.
    /// </summary>
    public sealed class GlobalCircuitHandler : CircuitHandler
    {
        private readonly ILogger<GlobalCircuitHandler> _logger;

        public GlobalCircuitHandler(ILogger<GlobalCircuitHandler> logger)
        {
            _logger = logger;
        }

        public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Blazor circuit opened: CircuitId={CircuitId}", circuit.Id);
            return Task.CompletedTask;
        }

        public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Blazor circuit connection up: CircuitId={CircuitId}", circuit.Id);
            return Task.CompletedTask;
        }

        public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Blazor circuit connection down: CircuitId={CircuitId}", circuit.Id);
            return Task.CompletedTask;
        }

        public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Blazor circuit closed: CircuitId={CircuitId}", circuit.Id);
            return Task.CompletedTask;
        }
    }
}