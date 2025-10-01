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
                PropertyNameCaseInsensitive = true, // Handles "Properties" vs "properties", "@t" vs "@T"
                ReadCommentHandling = JsonCommentHandling.Skip, // Skips comments if any
                AllowTrailingCommas = true // Allows trailing commas in JSON
            };

            // 1. Deserialize the incoming JSON message into the SerilogLogEvent structure
            var serilogEvent = JsonSerializer.Deserialize<SerilogLogEvent>(message, options);

            if (serilogEvent != null)
            {
                // 2. Map the deserialized SerilogLogEvent data to your database LogEntry entity
                var logEntry = new LogEntry
                {
                    Timestamp = serilogEvent.Timestamp,
                    Level = serilogEvent.Level,
                    MessageTemplate = serilogEvent.MessageTemplate,
                    Message = serilogEvent.Message, // Populated from SerilogLogEvent.Message (rendered message)

                    // Serialize Exception and Properties dictionary back to JSON strings for storage
                    Exception = serilogEvent.Exception != null ? JsonSerializer.Serialize(serilogEvent.Exception, options) : null,
                    // Serialize AdditionalProperties dictionary if present (extra props not mapped directly)
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
           
                // 4. Add the LogEntry to DbContext and Save
                _context.LogEntries.Add(logEntry);
                await _context.SaveChangesAsync();
            }
        }
    }
}
