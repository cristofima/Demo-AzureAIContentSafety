﻿using AutoMapper;
using AzureAIContentSafety.API.DTO.Requests;
using AzureAIContentSafety.API.DTO.Responses;
using AzureAIContentSafety.API.Entities;
using AzureAIContentSafety.API.Exceptions;
using AzureAIContentSafety.API.Interfaces;
using AzureAIContentSafety.API.Options;
using AzureAIContentSafety.API.Persistence;
using AzureAIContentSafety.API.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AzureAIContentSafety.API.Repositories;

public class PostRepository : IPostRepository
{
    private readonly IAzureStorageService azureStorageService;
    private readonly IAzureContentSafetyService azureContentSafetyService;
    private readonly AzureAIContentSafetyOptions azureAIContentSafetyOptions;
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public PostRepository(
        IAzureStorageService azureStorageService,
        IAzureContentSafetyService azureContentSafetyService,
        IOptions<AzureAIContentSafetyOptions> options,
        ApplicationDbContext context,
        IMapper mapper
    )
    {
        this.azureStorageService = azureStorageService;
        this.azureContentSafetyService = azureContentSafetyService;
        this.azureAIContentSafetyOptions = options.Value;
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<PostResponse> AddAsync(PostRequest request)
    {
        var post = new Post() { Text = request.Text, CreatedAt = DateTimeOffset.UtcNow };

        await this.AnalyzeTextAsync(post);
        await this.AnalyzeImageAsync(post, request);

        await this.context.Posts.AddAsync(post);
        await this.context.SaveChangesAsync();

        return this.mapper.Map<PostResponse>(post);
    }

    private async Task AnalyzeTextAsync(Post post)
    {
        if (!string.IsNullOrWhiteSpace(post.Text))
        {
            var textResult = await this.azureContentSafetyService.AnalyzeTextAsync(post.Text);
            if (textResult != null)
            {
                (
                    post.TextHateSeverity,
                    post.TextSelfHarmSeverity,
                    post.TextSexualSeverity,
                    post.TextViolenceSeverity
                ) = SeverityEvaluator.Evaluate(textResult.CategoriesAnalysis);

                var severities = new[]
                {
                    post.TextHateSeverity,
                    post.TextSelfHarmSeverity,
                    post.TextSexualSeverity,
                    post.TextViolenceSeverity,
                };

                var maxTextSeverity = severities.Max();

                post.TextIsHarmful =
                    maxTextSeverity >= azureAIContentSafetyOptions.TextSeverityThreshold.Blur;
                post.TextRequiresModeration =
                    maxTextSeverity >= azureAIContentSafetyOptions.TextSeverityThreshold.Reject;

                if (post.TextRequiresModeration)
                {
                    var index = severities.ToList().FindIndex(i => i == maxTextSeverity);
                    var severityName = index switch
                    {
                        0 => "Hate",
                        1 => "Self-Harm",
                        2 => "Sexual",
                        3 => "Violence",
                        _ => string.Empty,
                    };

                    throw new HarmfulContentException(
                        "Post requires moderation.",
                        new()
                        {
                            {
                                "Text",

                                [
                                    $"Text requires moderation in {severityName} content.",
                                    $"It has a severity level of {maxTextSeverity} out of 7.",
                                ]
                            },
                        }
                    );
                }
            }
        }
    }

    private async Task AnalyzeImageAsync(Post post, PostRequest request)
    {
        if (request.Image != null)
        {
            var memoryStream = await StreamUtil.ToMemoryStreamAsync(request.Image.OpenReadStream());
            var imageResult = await this.azureContentSafetyService.AnalyzeImageAsync(memoryStream);
            if (imageResult != null)
            {
                (
                    post.ImageHateSeverity,
                    post.ImageSelfHarmSeverity,
                    post.ImageSexualSeverity,
                    post.ImageViolenceSeverity
                ) = SeverityEvaluator.Evaluate(imageResult.CategoriesAnalysis);

                var severities = new[]
                {
                    post.ImageHateSeverity,
                    post.ImageSelfHarmSeverity,
                    post.ImageSexualSeverity,
                    post.ImageViolenceSeverity,
                };

                var maxImageSeverity = severities.Max();

                post.ImageIsHarmful =
                    maxImageSeverity >= azureAIContentSafetyOptions.ImageSeverityThreshold.Blur;
                post.ImageRequiresModeration =
                    maxImageSeverity >= azureAIContentSafetyOptions.ImageSeverityThreshold.Reject;

                if (post.ImageRequiresModeration)
                {
                    var index = severities.ToList().FindIndex(i => i == maxImageSeverity);
                    var severityName = index switch
                    {
                        0 => "Hate",
                        1 => "Self-Harm",
                        2 => "Sexual",
                        3 => "Violence",
                        _ => string.Empty,
                    };

                    throw new HarmfulContentException(
                        "Post requires moderation.",
                        new()
                        {
                            {
                                "Image",

                                [
                                    $"Image requires moderation in {severityName} content.",
                                    $"It has a severity level of {maxImageSeverity} out of 6.",
                                ]
                            },
                        }
                    );
                }

                post.ImagePath = await this.azureStorageService.UploadAsync(request.Image);
            }
        }
    }

    public async Task DeleteAsync(string id)
    {
        var post = await this.context.Posts.FindAsync(id);
        if (post is null)
        {
            throw new NotFoundException($"Post with ID {id} not found.");
        }

        if (!string.IsNullOrEmpty(post.ImagePath))
        {
            await this.azureStorageService.DeleteAsync(post.ImagePath);
        }

        this.context.Posts.Remove(post);
        await this.context.SaveChangesAsync();
    }

    public async Task<PaginatedList<PostResponse>> GetAll(int pageNumber, int pageSize)
    {
        var postQueryable = context
            .Posts.Where(p => !p.TextRequiresModeration && !p.ImageRequiresModeration)
            .OrderByDescending(p => p.CreatedAt)
            .AsNoTracking();

        var count = await postQueryable.CountAsync();

        var posts = await postQueryable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => this.mapper.Map<PostResponse>(p))
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(count / (double)pageSize);

        return new PaginatedList<PostResponse>(posts, pageNumber, totalPages, count);
    }

    public PostResponse GetById(string id)
    {
        var post = this.context.Posts.Find(id);
        if (post is null)
        {
            throw new NotFoundException($"Post with ID {id} not found.");
        }

        if (post is { TextRequiresModeration: false, ImageRequiresModeration: false })
        {
            return this.mapper.Map<PostResponse>(post);
        }
        else
        {
            throw new HarmfulContentException("Post requires moderation.");
        }
    }
}
