namespace AzureAIContentSafety.API.Entities
{
    public class Post
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string ImagePath { get; set; }
        public bool TextRequiresModeration { get; set; }
        public bool TextIsHarmful { get; set; }
        public int TextHateSeverity { get; set; }
        public int TextSelfHarmSeverity { get; set; }
        public int TextSexualSeverity { get; set; }
        public int TextViolenceSeverity { get; set; }
        public bool ImageRequiresModeration { get; set; }
        public bool ImageIsHarmful { get; set; }
        public int ImageHateSeverity { get; set; }
        public int ImageSelfHarmSeverity { get; set; }
        public int ImageSexualSeverity { get; set; }
        public int ImageViolenceSeverity { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? LastUpdatedAt { get; set; }
    }
}
