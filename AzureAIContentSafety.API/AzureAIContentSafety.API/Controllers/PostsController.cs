using AzureAIContentSafety.API.DTO.Requests;
using AzureAIContentSafety.API.DTO.Responses;
using AzureAIContentSafety.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AzureAIContentSafety.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class PostsController(IPostRepository postRepository) : ControllerBase
{
    private readonly IPostRepository postRepository = postRepository;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PostResponse>))]
    public IActionResult Get()
    {
        return Ok(this.postRepository.GetAll());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(string id)
    {
        return Ok(this.postRepository.GetById(id));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PostResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromForm] PostRequest request)
    {
        var post = await this.postRepository.AddAsync(request);
        return Ok(post);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        await this.postRepository.DeleteAsync(id);
        return NoContent();
    }
}
