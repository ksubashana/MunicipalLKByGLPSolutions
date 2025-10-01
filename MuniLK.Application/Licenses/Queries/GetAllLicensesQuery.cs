using MediatR;
using MuniLK.Domain.Entities;
using System.Collections.Generic;

public record GetAllLicensesQuery() : IRequest<IEnumerable<License>>;