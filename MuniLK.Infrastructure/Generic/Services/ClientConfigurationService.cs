using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MuniLK.Application.Generic.DTOs;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MuniLK.Infrastructure.Generic.Services
{
    public class ClientConfigurationService : IClientConfigurationService
    {
        private readonly MuniLKDbContext _db;
        private readonly ICurrentTenantService _tenantService;
        private readonly IMapper _mapper;

        public ClientConfigurationService(MuniLKDbContext db, ICurrentTenantService tenantService, IMapper mapper)
        {
            _db = db;
            _tenantService = tenantService;
            _mapper = mapper;
        }

        public async Task<ClientConfiguration> CreateAsync(ClientConfigurationCreateDto dto)
        {
            var tenantId = _tenantService.GetTenantId();

            var exists = await _db.ClientConfigurations
                .AnyAsync(c => c.TenantId == tenantId && c.ConfigKey == dto.ConfigKey);

            if (exists)
                throw new InvalidOperationException($"Config with key '{dto.ConfigKey}' already exists.");
            // Manual mapping instead of AutoMapper
            var entity = new ClientConfiguration
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                ConfigKey = dto.ConfigKey,
                ConfigJson = dto.ConfigJson.GetRawText(), // convert JsonElement to string
                LastUpdated = DateTimeOffset.UtcNow
            };
            //var entity = _mapper.Map<ClientConfiguration>(dto);
            //entity.TenantId = tenantId;
            //entity.LastUpdated = DateTimeOffset.UtcNow;

            _db.ClientConfigurations.Add(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public async Task<ClientConfigurationDto> UpdateAsync(ClientConfigurationUpdateDto dto)
        {
            var entity = await _db.ClientConfigurations.FindAsync(dto.Id);
            if (entity == null || entity.TenantId != _tenantService.GetTenantId())
                return null;

            if (entity.ConfigKey != dto.ConfigKey)
            {
                var duplicate = await _db.ClientConfigurations
                    .AnyAsync(c => c.TenantId == entity.TenantId && c.ConfigKey == dto.ConfigKey && c.Id != dto.Id);

                if (duplicate)
                    throw new InvalidOperationException($"Config with key '{dto.ConfigKey}' already exists.");
            }

            _mapper.Map(dto, entity);
            entity.LastUpdated = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync();

            return _mapper.Map<ClientConfigurationDto>(entity);
        }

        public async Task<ClientConfigurationDto?> GetByIdAsync(Guid id)
        {
            var entity = await _db.ClientConfigurations.FindAsync(id);
            if (entity == null || entity.TenantId != _tenantService.GetTenantId())
                return null;

            return _mapper.Map<ClientConfigurationDto>(entity);
        }

        public async Task<IEnumerable<ClientConfigurationDto>> GetAllAsync()
        {
            var tenantId = _tenantService.GetTenantId();
            var items = await _db.ClientConfigurations
                .Where(c => c.TenantId == tenantId)
                .ToListAsync();

            return _mapper.Map<List<ClientConfigurationDto>>(items);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.ClientConfigurations.FindAsync(id);
            if (entity == null || entity.TenantId != _tenantService.GetTenantId())
                throw new InvalidOperationException("Not found or access denied.");

            _db.ClientConfigurations.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
