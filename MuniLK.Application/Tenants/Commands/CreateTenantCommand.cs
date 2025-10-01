using MediatR;

namespace MuniLK.Application.Tenants.Commands
{
    public class CreateTenantCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Subdomain { get; set; }
        public string ContactEmail { get; set; }
    }
}
