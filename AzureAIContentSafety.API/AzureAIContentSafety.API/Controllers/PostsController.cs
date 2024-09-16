using AzureAIContentSafety.API.DTO.Requests;
using AzureAIContentSafety.API.DTO.Responses;
using AzureAIContentSafety.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AzureAIContentSafety.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository postRepository;
        public PostsController(IPostRepository postRepository) { 
            this.postRepository = postRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PostResponse>))]
        public IActionResult Get()
        {
            return Ok(this.postRepository.GetAll());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostResponse))]
        public IActionResult GetById(string id)
        {
            try
            {
                return Ok(this.postRepository.GetById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PostResponse))]
        public async Task<IActionResult> Create([FromForm] PostRequest request)
        {
            try
            {
                var post = await this.postRepository.AddAsync(request);
                return Ok(post);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await this.postRepository.DeleteAsync(id);
                return NoContent();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
