// MuniLK.Domain/Entities/LogEntry.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // For [Column]

namespace MuniLK.Domain.Entities
{
    /// <summary>
    /// Represents a log entry to be stored in the SQL Server database.
    /// This maps selected fields from the Serilog JSON message.
    /// </summary>
    public class LogEntry
    {
        [Key]
        public long Id { get; set; } // Primary Key (using long for potentially large log volumes)

        public DateTimeOffset Timestamp { get; set; }

        [MaxLength(128)] // Standard length for log levels (e.g., "Information", "Error")
        public string Level { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(max)")] // Use nvarchar(max) for potentially long rendered messages
        public string Message { get; set; } = string.Empty; // The final rendered log message

        [Column(TypeName = "nvarchar(max)")] // Store the original message template
        public string MessageTemplate { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(max)")] // Store exception details as a JSON string (nullable)
        public string? Exception { get; set; }

        [Column(TypeName = "nvarchar(max)")] // Store all other structured properties as a JSON string (nullable)
        public string? PropertiesJson { get; set; }

        // --- Extracted Common Properties for Easier Querying and Indexing ---
        // These are populated from the 'Properties' dictionary of the Serilog JSON
        [MaxLength(256)]
        public string? SourceContext { get; set; } // e.g., "Microsoft.AspNetCore.Hosting.Diagnostics"

        [MaxLength(256)]
        public string? RequestId { get; set; } // For correlating logs within a single HTTP request

        [MaxLength(256)]
        public string? RequestPath { get; set; } // e.g., "/api/License/GetAllLicenses"

        [MaxLength(256)]
        public string? MachineName { get; set; } // Name of the machine that logged the event

        public int? ThreadId { get; set; } // Thread ID where the log occurred

        // Specific HTTP-related properties from your sample log
        [MaxLength(256)]
        public string? Protocol { get; set; } // e.g., "HTTP/1.1"
        [MaxLength(10)]
        public string? Method { get; set; } // e.g., "GET", "POST"
        [MaxLength(256)]
        public string? Host { get; set; } // e.g., "localhost:5164"
        [MaxLength(256)]
        public string? ConnectionId { get; set; } // Identifier for the network connection

        // Distributed Tracing IDs
        public string? TraceId { get; set; } // @tr from Serilog JSON
        public string? SpanId { get; set; }  // @sp from Serilog JSON
        /// <summary>
        /// The TenantId associated with the log entry.
        /// </summary>
        public Guid? TenantId { get; set; } // Added TenantId property

        // Constructor for EF Core (optional, but good practice)
        public LogEntry() { }
    }
}
