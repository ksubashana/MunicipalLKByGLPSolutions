using MediatR;
using MuniLK.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class GetAllLicensesQueryHandler : IRequestHandler<GetAllLicensesQuery, IEnumerable<License>>
{
    private readonly ILicenseRepository _repository;

    public GetAllLicensesQueryHandler(ILicenseRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<License>> Handle(GetAllLicensesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}