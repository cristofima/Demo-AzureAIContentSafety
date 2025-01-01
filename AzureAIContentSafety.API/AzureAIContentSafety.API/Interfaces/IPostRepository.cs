using AzureAIContentSafety.API.DTO.Requests;
using AzureAIContentSafety.API.DTO.Responses;

namespace AzureAIContentSafety.API.Interfaces
{
    public interface IPostRepository
    {
        Task<PaginatedList<PostResponse>> GetAll(int pageNumber, int pageSize);
        PostResponse GetById(string id);
        Task<PostResponse> AddAsync(PostRequest request);
        Task DeleteAsync(string id);
    }
}
