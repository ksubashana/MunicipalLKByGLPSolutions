using MediatR;
using MuniLK.Application.Reports.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Reports.Queries
{
    public record GetReportQuery(Guid ReportId) : IRequest<ReportResponse?>;

}
