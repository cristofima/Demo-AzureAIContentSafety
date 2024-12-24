using Azure;
using Azure.AI.ContentSafety;
using AzureAIContentSafety.API.Interfaces;

namespace AzureAIContentSafety.API.Services;

public class AzureContentSafetyService : IAzureContentSafetyService
{
    private readonly ContentSafetyClient contentSafetyClient;

    public AzureContentSafetyService(IConfiguration configuration)
    {
        var endpoint = configuration.GetValue<string>("AzureAIContentSafety:endpoint");
        var apiKey = configuration.GetValue<string>("AzureAIContentSafety:apiKey");
        this.contentSafetyClient = new ContentSafetyClient(
            new Uri(endpoint),
            new AzureKeyCredential(apiKey)
        );
    }

    public async Task<AnalyzeTextResult> AnalyzeTextAsync(string text)
    {
        var request = new AnalyzeTextOptions(text)
        {
            OutputType = AnalyzeTextOutputType.EightSeverityLevels,
        };

        return await this.contentSafetyClient.AnalyzeTextAsync(request);
    }

    public async Task<AnalyzeImageResult> AnalyzeImageAsync(Stream imageStream)
    {
        var image = new ContentSafetyImageData(await BinaryData.FromStreamAsync(imageStream));
        var request = new AnalyzeImageOptions(image);

        return await this.contentSafetyClient.AnalyzeImageAsync(request);
    }
}
