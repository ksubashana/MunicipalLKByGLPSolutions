
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Microsoft.Extensions.Configuration;
using MuniLK.Application.Documents.Interfaces;
using MuniLK.Application.Documents.Queries.GetDocument;
using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

namespace MuniLK.Infrastructure.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName; // This will hold the container name from config

        // Constructor now correctly accepts IConfiguration to get the container name
        public BlobStorageService(BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _blobServiceClient = blobServiceClient;
            // Get the container name from configuration (e.g., from appsettings.json)
            // It's good practice to throw if a mandatory configuration is missing.

            _containerName = configuration["AzureBlobStorage:ContainerName"]
                             ?? throw new InvalidOperationException("Azure Blob Storage container name is not configured in appsettings.");
        }
        /// <summary>
        /// Internal helper method to get a BlobClient for a given blob path.
        /// This method also ensures the container exists before returning the client.
        /// </summary>
        /// <param name="blobPath">The full path of the blob within the container (e.g., "tenantId/documentId.ext").</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A configured BlobClient instance.</returns>
        private async Task<BlobClient> GetBlobClientInternal(string blobPath, CancellationToken cancellationToken)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            // Ensure the container exists. PublicAccessType.Blob allows anonymous read access,
            // which might be suitable for public documents. For private documents, use PublicAccessType.None
            // and implement SAS token generation for secure, temporary access.

            return containerClient.GetBlobClient(blobPath);
        }

        /// <summary>
        /// Uploads a file to blob storage.
        /// </summary>
        /// <param name="blobPath">The unique path/name for the blob within the container (e.g., "tenantId/documentId.ext").</param>
        /// <param name="content">The stream of the file content.</param>
        /// <param name="fileName">The original file name, stored as metadata.</param>
        /// <param name="contentType">The MIME type of the file (e.g., "application/pdf").</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The confirmed blob path/name used to store the file (should match input blobPath).</returns>
        public async Task<string> UploadAsync(
            string blobPath, // This is the full blob path (e.g., "tenantId/documentId.ext")
            Stream content,
            string fileName,
            string contentType,
            CancellationToken cancellationToken)
        {
            // Use the internal helper to get the BlobClient based on the provided blobPath
            var blobClient = await GetBlobClientInternal(blobPath, cancellationToken);

            var blobHttpHeader = new BlobHttpHeaders { ContentType = contentType };
            // Store original filename as metadata for later retrieval during download or analysis.
            var metadata = new Dictionary<string, string> { { "original_filename", fileName } };

            await blobClient.UploadAsync(
                content,
                new BlobUploadOptions { HttpHeaders = blobHttpHeader, Metadata = metadata },
                cancellationToken);

            // Return blobClient.Name, which is the full path within the container (e.g., "tenantId/documentId.ext")
            // This is what you'll store in your Document entity's BlobPath property.
            return blobClient.Name;
        }

        /// <summary>
        /// Downloads a file from blob storage.
        /// </summary>
        /// <param name="blobPath">The unique path/name of the blob within the container.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A tuple containing the file content stream, content type, and original file name, or null if not found.</returns>
        public async Task<(Stream Content, string ContentType, string FileName)?> DownloadAsync(
            string blobPath, // This blobPath is the one stored in your Document entity (e.g., "tenantId/documentId.ext")
            CancellationToken cancellationToken)
        {
            // Use the internal helper to get the BlobClient based on the provided blobPath
            var blobClient = await GetBlobClientInternal(blobPath, cancellationToken);

            if (!await blobClient.ExistsAsync(cancellationToken))
            {
                return null; // Document blob not found
            }

            // Get blob properties to retrieve content type and metadata
            var propertiesResponse = await blobClient.GetPropertiesAsync(cancellationToken: cancellationToken);
            // Download the content
            var response = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
            BlobProperties properties = propertiesResponse.Value;

            // Retrieve original filename from metadata; fallback to the blob's actual name if metadata missing
            var fileName = properties.Metadata.TryGetValue("original_filename", out var name) ? name : Path.GetFileName(blobPath);
            //var contentType = properties.ContentType;

            return (response.Value.Content, properties.ContentType, fileName);
        }

        /// <summary>
        /// Deletes a file from blob storage.
        /// </summary>
        /// <param name="blobPath">The unique path/name of the blob within the container.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task DeleteAsync(
            string blobPath, // This blobPath is the one stored in your Document entity
            CancellationToken cancellationToken)
        {
            // Use the internal helper to get the BlobClient based on the provided blobPath
            var blobClient = await GetBlobClientInternal(blobPath, cancellationToken);
            // Delete the blob, including any associated snapshots
            await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);
        }
    }
}