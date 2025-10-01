using MediatR;
using MuniLK.Application.LicensesLK.DTOs;
using MuniLK.Application.PropertiesLK.DTOs;
using MuniLK.Domain.Entities;

public record CreateLicenseCommand : IRequest<Guid?>
{
    public CreateLicenseRequest Request { get; set; }

    public CreateLicenseCommand(CreateLicenseRequest request)
    {
        Request = request;
    }
};