using AutoMapper;
using AzureAIContentSafety.API.DTO.Requests;
using AzureAIContentSafety.API.DTO.Responses;
using AzureAIContentSafety.API.Entities;
using AzureAIContentSafety.API.Interfaces;
using AzureAIContentSafety.API.Persistence;
using AzureAIContentSafety.API.Utils;

namespace AzureAIContentSafety.API.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IAzureStorageService azureStorageService;
        private readonly IAzureContentSafetyService azureContentSafetyService;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public PostRepository(IAzureStorageService azureStorageService, IAzureContentSafetyService azureContentSafetyService, ApplicationDbContext context, IMapper mapper)
        {
            this.azureStorageService = azureStorageService;
            this.azureContentSafetyService = azureContentSafetyService;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<PostResponse> AddAsync(PostRequest request)
        {
            var post = new Post()
            {
                Text = request.Text,
                CreatedAt = DateTime.UtcNow
            };

            var textResult = await this.azureContentSafetyService.AnalyzeTextAsync(request.Text);
            if (textResult != null)
            {
                (post.TextHateSeverity, post.TextSelfHarmSeverity, post.TextSexualSeverity, post.TextViolenceSeverity)
                    = SeverityEvaluator.Evaluate(textResult.CategoriesAnalysis);

                int maxTextSeverity = new[]
                    {
                        post.TextHateSeverity,
                        post.TextSelfHarmSeverity,
                        post.TextSexualSeverity,
                        post.TextViolenceSeverity
                    }.Max();

                post.TextIsHarmful = maxTextSeverity >= 4;
                post.TextRequiresModeration = maxTextSeverity >= 5;
            }

            if (request.Image != null)
            {
                post.ImagePath = await this.azureStorageService.UploadAsync(request.Image);
                var memoryStream = await StreamUtil.ToMemoryStreamAsync(request.Image.OpenReadStream());
                var imageResult = await this.azureContentSafetyService.AnalyzeImageAsync(memoryStream);
                if (imageResult != null)
                {
                    (post.ImageHateSeverity, post.ImageSelfHarmSeverity, post.ImageSexualSeverity, post.ImageViolenceSeverity) 
                        = SeverityEvaluator.Evaluate(imageResult.CategoriesAnalysis);

                    int maxImageSeverity = new[]
                       {
                            post.ImageHateSeverity,
                            post.ImageSelfHarmSeverity,
                            post.ImageSexualSeverity,
                            post.ImageViolenceSeverity
                        }.Max();

                    post.ImageIsHarmful = maxImageSeverity >= 4;
                    post.ImageRequiresModeration = maxImageSeverity >= 5;
                }
            }

            this.context.Posts.Add(post);
            this.context.SaveChanges();

            return this.mapper.Map<PostResponse>(post);
        }

        public async Task DeleteAsync(string id)
        {
            var post = this.context.Posts.Find(id);
            if (post != null)
            {
                if (!string.IsNullOrEmpty(post.ImagePath))
                {
                    await this.azureStorageService.DeleteAsync(post.ImagePath);
                }

                this.context.Posts.Remove(post);
                this.context.SaveChanges();
            }
        }

        public List<PostResponse> GetAll()
        {
            return this.context.Posts
                .Where(p => !p.TextRequiresModeration && !p.ImageRequiresModeration)
                .OrderByDescending(p  => p.CreatedAt)
                .Select(p => this.mapper.Map<PostResponse>(p))
                .ToList();
        }

        public PostResponse GetById(string id)
        {
            var post = this.context.Posts.Find(id);
            if(post != null && !post.TextRequiresModeration && !post.ImageRequiresModeration)
            {
                return this.mapper.Map<PostResponse>(post);
            }

            return null;
        }
    }
}
