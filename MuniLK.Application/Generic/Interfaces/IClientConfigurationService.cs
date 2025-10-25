using MuniLK.Application.Generic.DTOs;
using MuniLK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Generic.Interfaces
{
    /// <summary>
    /// Defines a service for managing client-specific configurations, handling global and tenant-specific settings.
    /// </summary>
    public interface IClientConfigurationService
    {
        Task<ClientConfiguration> CreateAsync(ClientConfigurationCreateDto dto);
        Task<ClientConfigurationDto> UpdateAsync(ClientConfigurationUpdateDto dto);
        Task<ClientConfigurationDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ClientConfigurationDto>> GetAllAsync();
        Task DeleteAsync(Guid id);
    }
}

