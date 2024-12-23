namespace AzureAIContentSafety.API.DTO.Requests;

public record PostRequest
{
    public string Text { get; init; }
    public IFormFile Image { get; init; }
}
