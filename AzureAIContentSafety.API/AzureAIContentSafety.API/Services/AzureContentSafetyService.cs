using Azure;
using Azure.AI.ContentSafety;
using AzureAIContentSafety.API.Interfaces;
using AzureAIContentSafety.API.Options;
using Microsoft.Extensions.Options;

namespace AzureAIContentSafety.API.Services;

public class AzureContentSafetyService(IOptions<AzureAIContentSafetyOptions> options)
    : IAzureContentSafetyService
{
    private readonly ContentSafetyClient _contentSafetyClient =
        new(new Uri(options.Value.Endpoint), new AzureKeyCredential(options.Value.ApiKey));

    public async Task<AnalyzeTextResult> AnalyzeTextAsync(string text)
    {
        var request = new AnalyzeTextOptions(text)
        {
            OutputType = AnalyzeTextOutputType.EightSeverityLevels,
        };

        return await _contentSafetyClient.AnalyzeTextAsync(request);
    }

    public async Task<AnalyzeImageResult> AnalyzeImageAsync(Stream imageStream)
    {
        var image = new ContentSafetyImageData(await BinaryData.FromStreamAsync(imageStream));
        var request = new AnalyzeImageOptions(image);

        return await _contentSafetyClient.AnalyzeImageAsync(request);
    }
}
