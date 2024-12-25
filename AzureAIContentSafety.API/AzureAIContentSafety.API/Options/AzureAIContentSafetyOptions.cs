using System.ComponentModel.DataAnnotations;

namespace AzureAIContentSafety.API.Options
{
    public class AzureAIContentSafetyOptions
    {
        [Required]
        [Url]
        public string Endpoint { get; init; }

        [Required]
        public string ApiKey { get; init; }

        [Required]
        public SeverityThreshold TextSeverityThreshold { get; init; }

        [Required]
        public SeverityThreshold ImageSeverityThreshold { get; init; }
    }

    public class SeverityThreshold
    {
        [Required]
        [Range(0, 7)]
        public int Blur { get; init; }

        [Range(0, 7)]
        [Required]
        public int Reject { get; init; }
    }
}
