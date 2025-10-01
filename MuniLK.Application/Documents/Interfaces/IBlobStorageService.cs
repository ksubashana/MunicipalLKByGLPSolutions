// MuniLK.Application.Documents.Interfaces/IBlobStorageService.cs
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.Documents.Interfaces
{
    public interface IBlobStorageService
    {
        /// <summary>
        /// Uploads a file to blob storage.
        /// The blobPath should include tenant and document identifiers (e.g., "tenantId/documentId.ext").
        /// </summary>
        /// <param name="blobPath">The unique path/name for the blob within the container (e.g., "tenantId/documentId.ext").</param>
        /// <param name="content">The stream of the file content.</param>
        /// <param name="fileName">The original file name, stored as metadata.</param>
        /// <param name="contentType">The MIME type of the file (e.g., "application/pdf").</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The confirmed blob path/name used to store the file (should match input blobPath).</returns>
        Task<string> UploadAsync(
            string blobPath,
            Stream content,
            string fileName,
            string contentType,
            CancellationToken cancellationToken);

        /// <summary>
        /// Downloads a file from blob storage.
        /// </summary>
        /// <param name="blobPath">The unique path/name of the blob within the container.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A tuple containing the file content stream, content type, and original file name, or null if not found.</returns>
        Task<(Stream Content, string ContentType, string FileName)?> DownloadAsync(
            string blobPath,
            CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a file from blob storage.
        /// </summary>
        /// <param name="blobPath">The unique path/name of the blob within the container.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task DeleteAsync(
            string blobPath,
            CancellationToken cancellationToken);
    }
}