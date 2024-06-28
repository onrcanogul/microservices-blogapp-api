using MediatR;
using Microsoft.AspNetCore.Mvc;
using Post.Service.Features.Commands.CreatePost;
using Post.Service.Features.Commands.DeletePost;
using Post.Service.Features.Commands.UpdatePost;
using Post.Service.Features.Queries.GetPostById;
using Post.Service.Features.Queries.GetPostByUserId;
using Post.Service.Features.Queries.GetPosts;

namespace Post.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            GetPostsQueryResponse response = await mediator.Send(new GetPostsQueryRequest());
            return Ok(response);
        }
        [HttpGet("{PostId}")]
        public async Task<IActionResult> GetPostById([FromRoute] GetPostByIdQueryRequest request)
        {
            GetPostByIdQueryResponse response = await mediator.Send(request);
            return Ok(response);
        }
        [HttpGet("[action]/{UserId}")]
        public async Task<IActionResult> GetPostByUser([FromRoute] GetPostByUserIdQueryRequest request)
        {
            GetPostByUserIdQueryResponse response = await mediator.Send(request);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostCommandRequest request)
        {
            CreatePostCommandResponse response = await mediator.Send(request);
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> UpdatePost(UpdatePostCommandRequest request)
        {
            UpdatePostCommandResponse response = await mediator.Send(request);
            return Ok(response);
        }
        [HttpDelete("{PostId}")]
        public async Task<IActionResult> DeletePost([FromRoute]DeletePostCommandRequest request)
        {
            DeletePostCommandResponse response = await mediator.Send(request);
            return Ok(response);    
        }
    }
}
