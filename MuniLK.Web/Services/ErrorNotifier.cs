using System;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Web.Services
{
    public enum ErrorSeverity { Info, Success, Warning, Error, Critical }

    public class ErrorNotifier
    {
        public event Func<ErrorInfo, Task>? OnError;
        public ErrorInfo? LastError { get; private set; }

        // Existing exception-based notification
        public Task NotifyAsync(Exception ex, string? context = null, ErrorSeverity severity = ErrorSeverity.Error)
        {
            LastError = new ErrorInfo
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace ?? string.Empty,
                Context = context,
                TimestampUtc = DateTime.UtcNow,
                Severity = severity,
                IsException = true
            };
            return OnError?.Invoke(LastError) ?? Task.CompletedTask;
        }

        // New message-only notification (no exception)
        public Task NotifyMessageAsync(string message, string? context = null, ErrorSeverity severity = ErrorSeverity.Info)
        {
            LastError = new ErrorInfo
            {
                Message = message,
                StackTrace = string.Empty,
                Context = context,
                TimestampUtc = DateTime.UtcNow,
                Severity = severity,
                IsException = false
            };
            return OnError?.Invoke(LastError) ?? Task.CompletedTask;
        }

        public void Clear() => LastError = null;
    }

    public class ErrorInfo
    {
        public string Message { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty;
        public string? Context { get; set; }
        public DateTime TimestampUtc { get; set; }
        public ErrorSeverity Severity { get; set; } = ErrorSeverity.Error;
        public bool IsException { get; set; }
    }
}
