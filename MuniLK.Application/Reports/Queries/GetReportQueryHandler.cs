using MediatR;
using MuniLK.Application.Reports.DTOs;
using MuniLK.Application.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Reports.Queries
{
    public class GetReportQueryHandler : IRequestHandler<GetReportQuery, ReportResponse?>
    {
        private readonly IReportRepository _repository;

        public GetReportQueryHandler(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<ReportResponse?> Handle(GetReportQuery request, CancellationToken cancellationToken)
        {
            var report = await _repository.GetByIdAsync(request.ReportId);
            if (report == null) return null;

            return new ReportResponse
            {
                Id = report.Id,
                Name = report.Name,
                Description = report.Description,
                BlobPath = report.BlobPath
            };
        }
    }

}
