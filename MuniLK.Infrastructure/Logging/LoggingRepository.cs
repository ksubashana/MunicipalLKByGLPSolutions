using Microsoft.Extensions.Logging;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MuniLK.Domain.Models;
using MuniLK.Application.Generic.Interfaces;

namespace MuniLK.Infrastructure.Logging
{
    public class LoggingRepository : IMyMessageProcessor
    {
        private readonly MuniLKDbContext _context;
        public LoggingRepository(MuniLKDbContext context)
        {
            _context = context;
        }
        public async Task ProcessMessageAsync(string message)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            SerilogLogEvent? serilogEvent = null;
            try
            {
                serilogEvent = JsonSerializer.Deserialize<SerilogLogEvent>(message, options);
            }
            catch (Exception)
            {
                // Fallback minimal parse
                using var doc = JsonDocument.Parse(message);
                var root = doc.RootElement;

                // Create minimal LogEntry directly
                var fallbackEntry = new LogEntry
                {
                    Timestamp = root.TryGetProperty("@t", out var t) && t.ValueKind == JsonValueKind.String
                        ? DateTimeOffset.Parse(t.GetString()!)
                        : DateTimeOffset.UtcNow,
                    Level = root.TryGetProperty("@l", out var l) ? l.GetString() ?? "Error" : "Error",
                    Message = root.TryGetProperty("@m", out var m) ? m.GetString() ?? string.Empty : string.Empty,
                    MessageTemplate = root.TryGetProperty("@mt", out var mt) ? mt.GetString() ?? string.Empty : string.Empty,
                    Exception = root.TryGetProperty("@x", out var x) ? x.GetString() : null,
                    PropertiesJson = message, // store raw for later inspection
                    SourceContext = root.TryGetProperty("SourceContext", out var sc) ? sc.GetString() : null,
                    RequestId = root.TryGetProperty("RequestId", out var rid) ? rid.GetString() : null,
                    RequestPath = root.TryGetProperty("RequestPath", out var rpath) ? rpath.GetString() : null,
                    Host = root.TryGetProperty("Host", out var host) ? host.GetString() : null,
                    Method = root.TryGetProperty("Method", out var method) ? method.GetString() : null,
                    Protocol = root.TryGetProperty("Protocol", out var proto) ? proto.GetString() : null,
                    ConnectionId = root.TryGetProperty("ConnectionId", out var cid) ? cid.GetString() : null,
                    TraceId = root.TryGetProperty("@tr", out var tr) ? tr.GetString() : null,
                    SpanId = root.TryGetProperty("@sp", out var sp) ? sp.GetString() : null,
                    TenantId = root.TryGetProperty("TenantId", out var ten) && Guid.TryParse(ten.GetString(), out var g) ? g : null
                };

                _context.LogEntries.Add(fallbackEntry);
                await _context.SaveChangesAsync();
                return;
            }

            if (serilogEvent == null) return;

            var logEntry = new LogEntry
            {
                Timestamp = serilogEvent.Timestamp,
                Level = serilogEvent.Level,
                MessageTemplate = serilogEvent.MessageTemplate,
                Message = serilogEvent.Message,
                Exception = serilogEvent.Exception != null ? JsonSerializer.Serialize(serilogEvent.Exception, options) : null,
                PropertiesJson = serilogEvent.AdditionalProperties != null
                    ? JsonSerializer.Serialize(serilogEvent.AdditionalProperties, options)
                    : null,
                SourceContext = serilogEvent.SourceContext,
                RequestId = serilogEvent.RequestId,
                RequestPath = serilogEvent.RequestPath,
                MachineName = serilogEvent.AdditionalProperties != null && serilogEvent.AdditionalProperties.TryGetValue("MachineName", out var machineName) ? machineName?.ToString() : null,
                ThreadId = serilogEvent.AdditionalProperties != null && serilogEvent.AdditionalProperties.TryGetValue("ThreadId", out var threadIdObj) && int.TryParse(threadIdObj?.ToString(), out var threadId) ? threadId : (int?)null,
                Protocol = serilogEvent.Protocol,
                Method = serilogEvent.Method,
                Host = serilogEvent.Host,
                ConnectionId = serilogEvent.ConnectionId,
                TraceId = serilogEvent.TraceId,
                SpanId = serilogEvent.SpanId,
                TenantId = serilogEvent.TenantId
            };

            _context.LogEntries.Add(logEntry);
            await _context.SaveChangesAsync();
        }
    }
}
