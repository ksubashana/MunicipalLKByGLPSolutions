using MuniLK.Application.Services.DTOs;

namespace MuniLK.Web.Interfaces
{
 public interface IModuleService
    {
        Task<Guid> GetModuleIdByCodeAsync(string Code);
    }
}
