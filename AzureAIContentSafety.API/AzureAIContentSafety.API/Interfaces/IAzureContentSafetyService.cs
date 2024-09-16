using Azure.AI.ContentSafety;

namespace AzureAIContentSafety.API.Interfaces
{
    public interface IAzureContentSafetyService
    {
        Task<AnalyzeTextResult> AnalyzeTextAsync(string text);
        Task<AnalyzeImageResult> AnalyzeImageAsync(Stream imageStream);
    }
}
