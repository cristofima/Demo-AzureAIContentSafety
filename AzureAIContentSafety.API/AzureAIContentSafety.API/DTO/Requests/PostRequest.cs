using System.ComponentModel.DataAnnotations;

namespace AzureAIContentSafety.API.DTO.Requests
{
    public class PostRequest
    {
        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Text { get; set; }
        public IFormFile Image { get; set; }
    }
}
