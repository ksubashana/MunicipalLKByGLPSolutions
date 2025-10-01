using MediatR;
using MuniLK.Application.Documents.Interfaces;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.Reports.Interfaces;
using MuniLK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Reports.Commands.UploadReport
{
    public class UploadReportCommandHandler : IRequestHandler<UploadReportCommand, Guid?>
    {
        private readonly IReportRepository _repository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ICurrentTenantService _tenantService;
        private readonly ICurrentUserService _userService;

        public UploadReportCommandHandler(
            IReportRepository repository,
            IBlobStorageService blobStorageService,
            ICurrentTenantService tenantService,
            ICurrentUserService userService)
        {
            _repository = repository;
            _blobStorageService = blobStorageService;
            _tenantService = tenantService;
            _userService = userService;
        }

        public async Task<Guid?> Handle(UploadReportCommand command, CancellationToken cancellationToken)
        {
            var tenantId = _tenantService.GetTenantId();
            if (!tenantId.HasValue)
                throw new UnauthorizedAccessException();

            var request = command.Request;
            var reportId = Guid.NewGuid();
            var extension = Path.GetExtension(request.File.FileName)?.ToLowerInvariant() ?? "";
            var blobPath = $"{tenantId.Value}/reports/{reportId}{extension}";

            await _blobStorageService.UploadAsync(
                blobPath,
                request.File.OpenReadStream(),
                request.File.FileName,
                request.File.ContentType,
                cancellationToken);

            var report = new Report
            {
                Id = reportId,
                Name = request.Name,
                Description = request.Description,
                BlobPath = blobPath,
                TenantId = tenantId,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _userService.UserName
            };

            await _repository.AddAsync(report);
            return report.Id;
        }
    }

}
