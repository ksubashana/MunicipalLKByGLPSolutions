using MuniLK.Application.Documents.DTOs;
using System.Net.Http;

namespace MuniLK.Web.Clients
{
    public class DocumentClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DocumentClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<(string FileName, string Base64)> DownloadDocumentAsync(DocumentLinkResponse document, CancellationToken ct = default)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AuthorizedClient");
                var downloadUrl = $"api/Documents/{document.DocumentId}/download";

                using var response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead, ct);
                if (!response.IsSuccessStatusCode)
                    return (string.Empty, string.Empty);

                using var stream = await response.Content.ReadAsStreamAsync(ct);
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms, ct);
                var base64 = Convert.ToBase64String(ms.ToArray());
                return (document.FileName, base64);
            }
            catch
            {
                return (string.Empty, string.Empty);
            }
        }

        public string GetDocumentPreviewUrl(Guid documentId)
        {
            // TODO: move base URL to configuration
            return $"http://localhost:5164/api/Documents/{documentId}/preview";
        }

        public bool IsImageFile(string extension)
        {
            var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            return imageExtensions.Contains(extension.ToLowerInvariant());
        }

        public bool IsPdfFile(string extension)
        {
            return extension.ToLowerInvariant() == ".pdf";
        }
        public bool IsWordFile(string extension)
        {
            var docExtensions = new[] { ".doc", ".docx", ".csv", ".xlsx" };
            return docExtensions.Contains(extension.ToLowerInvariant());
        }
        public string FormatFileSize(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB" };
            int counter = 0;
            decimal number = bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return $"{number:n1} {suffixes[counter]}";
        }
    }
}
