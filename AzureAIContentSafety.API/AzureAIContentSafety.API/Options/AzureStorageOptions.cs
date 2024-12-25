using System.ComponentModel.DataAnnotations;

namespace AzureAIContentSafety.API.Options;

public class AzureStorageOptions
{
    [Required]
    public string ConnectionString { get; init; }

    [Required]
    public string BlobCacheControl { get; init; }

    [Required]
    public string BlobContainerName { get; init; }
}
