using AutoMapper;
using AzureAIContentSafety.API.DTO.Requests;
using AzureAIContentSafety.API.DTO.Responses;
using AzureAIContentSafety.API.Entities;

namespace AzureAIContentSafety.API.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Post, PostResponse>();
            CreateMap<PostRequest, Post>();
        }
    }
}
