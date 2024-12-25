using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureAIContentSafety.API.Interfaces;
using AzureAIContentSafety.API.Options;
using Microsoft.Extensions.Options;

namespace AzureAIContentSafety.API.Services;

public class AzureStorageService(IOptions<AzureStorageOptions> options) : IAzureStorageService
{
    private readonly AzureStorageOptions _azureStorageOptions = options.Value;

    public async Task DeleteAsync(string blobFilename)
    {
        var blobContainerClient = new BlobContainerClient(
            _azureStorageOptions.ConnectionString,
            _azureStorageOptions.BlobContainerName
        );
        var blobClient = blobContainerClient.GetBlobClient(blobFilename);

        try
        {
            await blobClient.DeleteAsync();
        }
        catch { }
    }

    public async Task<string> UploadAsync(IFormFile file, string blobName = null)
    {
        if (file.Length == 0)
        {
            return null;
        }

        var blobContainerClient = new BlobContainerClient(
            _azureStorageOptions.ConnectionString,
            _azureStorageOptions.BlobContainerName
        );

        // Get a reference to the blob just uploaded from the API in a container from configuration settings
        if (string.IsNullOrEmpty(blobName))
        {
            blobName = Guid.NewGuid().ToString();
        }

        var blobClient = blobContainerClient.GetBlobClient(blobName);

        var blobHttpHeader = new BlobHttpHeaders
        {
            ContentType = file.ContentType,
            CacheControl = _azureStorageOptions.BlobCacheControl,
        };

        // Open a stream for the file we want to upload
        await using var stream = file.OpenReadStream();

        // Upload the file async
        await blobClient.UploadAsync(
            stream,
            new BlobUploadOptions { HttpHeaders = blobHttpHeader }
        );

        return blobName;
    }
}
