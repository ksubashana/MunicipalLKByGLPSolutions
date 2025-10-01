// MuniLK.Consumer/Models/SerilogModels.cs
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MuniLK.Domain.Models
{
    /// <summary>
    /// Represents the structured data of a Serilog log event as it appears in the RabbitMQ message.
    /// This matches the JSON output format of Serilog.
    /// </summary>
    public class SerilogLogEvent
    {
        // Serilog's default timestamp property is "@t"
        [JsonPropertyName("@t")]
        public DateTimeOffset Timestamp { get; set; }

        // Serilog's default message template property is "@mt"
        [JsonPropertyName("@mt")]
        public string MessageTemplate { get; set; } = string.Empty;

        // The rendered message. Serilog's CompactJsonFormatter often puts this in a top-level "Message" property.
        // If your formatter doesn't, this might be empty after deserialization, and you'd reconstruct it.
        [JsonPropertyName("@m")]
        public string Message { get; set; } = string.Empty; // This will hold the rendered message
        [JsonPropertyName("@l")]
        public string Level { get; set; } = string.Empty; // e.g., "Information", "Error"
        [JsonPropertyName("@i")]
        public string? EventIdString { get; set; } // sometimes an id string
        [JsonPropertyName("@x")]
        public SerilogException? Exception { get; set; } // Nullable for logs without exceptions

        // All other structured data (like Protocol, Method, RequestId, SourceContext)
        // goes into the Properties dictionary.
        public string? Protocol { get; set; }
        public string? Method { get; set; }
        public string? ContentType { get; set; }
        public long? ContentLength { get; set; }
        public string? Scheme { get; set; }
        public string? Host { get; set; }
        public string? PathBase { get; set; }
        public string? Path { get; set; }
        public string? QueryString { get; set; }
        public string? SourceContext { get; set; }
        public string? RequestId { get; set; }
        public string? RequestPath { get; set; }
        public string? ConnectionId { get; set; }
        // Top-level properties from Serilog's tracing/enrichment

        // Nested event id object
        public EventId? EventId { get; set; }

        // Catch-all for any additional properties
        [JsonExtensionData]
        public Dictionary<string, object>? AdditionalProperties { get; set; }

        [JsonPropertyName("@tr")]
        public string? TraceId { get; set; } // Distributed tracing Trace ID

        [JsonPropertyName("@sp")]
        public string? SpanId { get; set; }  // Distributed tracing Span ID
        /// <summary>
        /// The TenantId property added by the custom enricher.
        /// </summary>
        [JsonPropertyName("TenantId")] // Map to the "TenantId" property added by the enricher
        public Guid? TenantId { get; set; } // Added TenantId property
    }

    /// <summary>
    /// Represents the structure of an exception within a Serilog log event.
    /// </summary>
    public class SerilogException
    {
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty;
        public SerilogException? InnerException { get; set; } // For nested exceptions
    }

    public class EventId
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
