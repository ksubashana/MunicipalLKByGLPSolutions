using MediatR;
using MuniLK.Domain.Entities;

public record GetLicenseByIdQuery(Guid Id) : IRequest<License?>;