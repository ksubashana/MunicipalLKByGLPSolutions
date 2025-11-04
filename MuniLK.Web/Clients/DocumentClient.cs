using MuniLK.Application.Documents.DTOs;
using System.Net.Http;
using MuniLK.Web.Services;

namespace MuniLK.Web.Clients
{
    public class DocumentClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ErrorNotifier _errorNotifier;

        public DocumentClient(IHttpClientFactory httpClientFactory, ErrorNotifier errorNotifier)
        {
            _httpClientFactory = httpClientFactory;
            _errorNotifier = errorNotifier;
        }

        public async Task<(string FileName, string Base64)> DownloadDocumentAsync(DocumentLinkResponse document, CancellationToken ct = default)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AuthorizedClient");
                var downloadUrl = $"api/Documents/{document.DocumentId}/download";

                using var response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead, ct);
                if (!response.IsSuccessStatusCode)
                {
                    await _errorNotifier.NotifyMessageAsync($"Download failed: {response.StatusCode}", "DocumentClient", ErrorSeverity.Warning);
                    return (string.Empty, string.Empty);
                }

                using var stream = await response.Content.ReadAsStreamAsync(ct);
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms, ct);
                var base64 = Convert.ToBase64String(ms.ToArray());
                await _errorNotifier.NotifyMessageAsync("Document downloaded", "DocumentClient", ErrorSeverity.Info);
                return (document.FileName, base64);
            }
            catch (Exception ex)
            {
                await _errorNotifier.NotifyAsync(ex, "DocumentClient Download", ErrorSeverity.Error);
                return (string.Empty, string.Empty);
            }
        }

        public string GetDocumentPreviewUrl(Guid documentId) => $"http://localhost:5164/api/Documents/{documentId}/preview";

        public bool IsImageFile(string extension) => new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" }.Contains(extension.ToLowerInvariant());
        public bool IsPdfFile(string extension) => extension.ToLowerInvariant() == ".pdf";
        public bool IsWordFile(string extension) => new[] { ".doc", ".docx", ".csv", ".xlsx" }.Contains(extension.ToLowerInvariant());
        public string FormatFileSize(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB" };
            int counter = 0; decimal number = bytes;
            while (Math.Round(number / 1024) >= 1) { number /= 1024; counter++; }
            return $"{number:n1} {suffixes[counter]}";
        }
    }
}
