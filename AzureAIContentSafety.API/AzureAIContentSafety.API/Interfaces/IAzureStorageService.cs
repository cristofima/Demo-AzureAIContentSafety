namespace AzureAIContentSafety.API.Interfaces
{
    public interface IAzureStorageService
    {
        Task<string> UploadAsync(IFormFile file, string blobName = null);
        Task DeleteAsync(string blobFilename);
    }
}
