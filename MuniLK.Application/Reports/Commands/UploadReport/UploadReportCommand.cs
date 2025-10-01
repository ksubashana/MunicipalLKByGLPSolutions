using MediatR;
using MuniLK.Application.Reports.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Reports.Commands.UploadReport
{
    public record UploadReportCommand(UploadReportRequest Request) : IRequest<Guid?>;

}
