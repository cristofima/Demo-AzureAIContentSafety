using AzureAIContentSafety.API.DTO.Requests;
using AzureAIContentSafety.API.DTO.Responses;

namespace AzureAIContentSafety.API.Interfaces
{
    public interface IPostRepository
    {
        List<PostResponse> GetAll();
        PostResponse GetById(string id);
        Task<PostResponse> AddAsync(PostRequest request);
        Task DeleteAsync(string id);
    }
}
