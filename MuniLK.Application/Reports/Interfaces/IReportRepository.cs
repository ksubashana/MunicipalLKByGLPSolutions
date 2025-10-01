using MuniLK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Reports.Interfaces
{
    public interface IReportRepository
    {
        Task AddAsync(Report report);
        Task<Report?> GetByIdAsync(Guid id);
        Task<List<Report>> GetAllAsync(Guid? tenantId);
        Task DeleteAsync(Guid id);
    }

}
