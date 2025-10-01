using Microsoft.Extensions.DependencyInjection;
using MuniLK.Application.Generic.Interfaces;
using Serilog.Core;
using Serilog.Events;

namespace MuniLK.Worker.Services
{
    public class TenantIdEnricher : ILogEventEnricher
    {
        private readonly IServiceProvider _serviceProvider;

        public TenantIdEnricher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            using var scope = _serviceProvider.CreateScope();
            var tenantService = scope.ServiceProvider.GetRequiredService<ICurrentTenantService>();
            var tenantId = tenantService.GetTenantId() ?? Guid.Empty;
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TenantId", tenantId));
        }
    }
}