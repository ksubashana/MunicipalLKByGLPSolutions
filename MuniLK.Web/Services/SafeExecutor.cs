using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MuniLK.Web.Services
{
    public interface ISafeExecutor
    {
        Task<bool> Run(Func<Task> action, string context, ErrorSeverity defaultSeverity = ErrorSeverity.Error, Func<Exception, string?>? enrich = null);
    }

    public class SafeExecutor : ISafeExecutor
    {
        private readonly ILogger<SafeExecutor> _logger;
        private readonly ErrorNotifier _notifier;

        public SafeExecutor(ILogger<SafeExecutor> logger, ErrorNotifier notifier)
        {
            _logger = logger;
            _notifier = notifier;
        }

        public async Task<bool> Run(Func<Task> action, string context, ErrorSeverity defaultSeverity = ErrorSeverity.Error, Func<Exception, string?>? enrich = null)
        {
            try
            {
                await action();
                return true;
            }
            catch (Exception ex)
            {
                var severity = Classify(ex, defaultSeverity);
                _logger.LogError(ex, "{Context} failed", context);
                await _notifier.NotifyAsync(ex, context, severity);
                var detail = enrich?.Invoke(ex);
                if (!string.IsNullOrWhiteSpace(detail))
                {
                    await _notifier.NotifyMessageAsync(Trim(detail), context, severity);
                }
                return false;
            }
        }

        private static ErrorSeverity Classify(Exception ex, ErrorSeverity fallback)
        {
            if (ex is HttpRequestException httpEx)
            {
                var match = Regex.Match(httpEx.Message, @"Status\s+(\d{3})");
                if (match.Success && int.TryParse(match.Groups[1].Value, out var code))
                {
                    if (code >= 500) return ErrorSeverity.Error; // red
                    if (code == 401 || code == 403) return ErrorSeverity.Info; // auth issues
                    if (code == 404) return ErrorSeverity.Info; // not found
                    if (code >= 400) return ErrorSeverity.Warning; // client validation etc.
                }
                return ErrorSeverity.Warning;
            }
            return ex switch
            {
                OperationCanceledException => ErrorSeverity.Info,
                TimeoutException => ErrorSeverity.Warning,
                _ => fallback
            };
        }

        private static string Trim(string raw)
        {
            raw = raw.Replace('\r', ' ').Replace('\n', ' ');
            return raw.Length > 500 ? raw.Substring(0, 500) + "…" : raw;
        }
    }
}
