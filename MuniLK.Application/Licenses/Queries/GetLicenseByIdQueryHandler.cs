using MediatR;
using MuniLK.Domain.Entities;

public class GetLicenseByIdQueryHandler : IRequestHandler<GetLicenseByIdQuery, License?>
{
    private readonly ILicenseRepository _repository;

    public GetLicenseByIdQueryHandler(ILicenseRepository repository)
    {
        _repository = repository;
    }

    public async Task<License?> Handle(GetLicenseByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.Id, cancellationToken);
    }
}