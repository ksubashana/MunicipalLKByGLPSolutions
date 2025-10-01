using Microsoft.EntityFrameworkCore;
using MuniLK.Application.FeatureId.Interfaces;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MuniLK.Infrastructure.FeatureIDService
{
    public class FeatureIDService : IFeatureIdService
    {
        private readonly MuniLKDbContext _context;
        private readonly ICurrentTenantService _currentTenantService;

        public FeatureIDService(MuniLKDbContext context, ICurrentTenantService currentTenantService)
        {
            _context = context;
            _currentTenantService = currentTenantService;
        }

        public async Task<string> GenerateFeatureIdAsync(string configKey, CancellationToken cancellationToken)
        {
            var tenantId = _currentTenantService.GetTenantId();

            var config = await _context.ClientConfigurations
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.TenantId == tenantId && c.ConfigKey == configKey, cancellationToken);

            if (config == null)
                throw new Exception($"Configuration not found for key: {configKey}");

            var json = JsonDocument.Parse(config.ConfigJson).RootElement;

            var parts = json.TryGetProperty("Parts", out var partsElement)
                ? partsElement.EnumerateArray().ToList()
                : throw new Exception($"'Parts' array not found in config for key: {configKey}");

            // Default values
            var yearFormat = "yyyy";
            var sequencePadLength = 4;
            var currentYear = DateTime.UtcNow.ToString(yearFormat);

            // Get current sequence year format and pad length (we'll update below as needed)
            foreach (var part in parts)
            {
                var type = part.GetProperty("Type").GetString();
                if (type == "Year" && part.TryGetProperty("Format", out var formatProp))
                {
                    yearFormat = formatProp.GetString() ?? "yyyy";
                    currentYear = DateTime.UtcNow.ToString(yearFormat);
                }
                else if (type == "Sequence" && part.TryGetProperty("PadLength", out var padProp))
                {
                    sequencePadLength = padProp.GetInt32();
                }
            }

            // Get latest sequence number for this year + configKey
            var lastEntry = await _context.FeatureIdAudit
                .Where(f => f.TenantId == tenantId && f.ConfigKey == configKey && f.Year == currentYear)
                .OrderByDescending(f => f.SequenceNumber)
                .FirstOrDefaultAsync(cancellationToken);

            var nextSeq = (lastEntry?.SequenceNumber ?? 0) + 1;
            var seqFormatted = nextSeq.ToString().PadLeft(sequencePadLength, '0');

            // Save audit
            _context.FeatureIdAudit.Add(new FeatureIdAudit
            {
                ConfigKey = configKey,
                TenantId = tenantId,
                SequenceNumber = nextSeq,
                Year = currentYear,
                GeneratedAt = DateTime.UtcNow
            });

            //await _context.SaveChangesAsync(cancellationToken); Handled with the UNITOFWORK

            // Build the final ID based on Parts
            var finalIdParts = new List<string>();

            foreach (var part in parts)
            {
                var type = part.GetProperty("Type").GetString();
                switch (type)
                {
                    case "Static":
                        if (part.TryGetProperty("Value", out var staticVal))
                            finalIdParts.Add(staticVal.GetString() ?? "");
                        break;

                    case "Year":
                        finalIdParts.Add(currentYear);
                        break;

                    case "Sequence":
                        finalIdParts.Add(seqFormatted);
                        break;

                    case "Separator":
                        if (part.TryGetProperty("Value", out var sepVal))
                            finalIdParts.Add(sepVal.GetString() ?? "-");
                        else
                            finalIdParts.Add("-");
                        break;
                }
            }

            return string.Join("", finalIdParts);
        }

    }
}

