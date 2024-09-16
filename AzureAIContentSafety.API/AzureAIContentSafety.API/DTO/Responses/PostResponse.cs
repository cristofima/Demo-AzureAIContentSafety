namespace AzureAIContentSafety.API.DTO.Responses
{
    public class PostResponse
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string ImagePath { get; set; }
        public bool TextIsHarmful { get; set; }
        public bool ImageIsHarmful { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
