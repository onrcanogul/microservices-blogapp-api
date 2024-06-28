using Comment.Service.Features.Commands.CreateComment;
using Comment.Service.Features.Commands.DeleteComment;
using Comment.Service.Features.Queries.GetCommentById;
using Comment.Service.Features.Queries.GetCommentsByPost;
using Comment.Service.Features.Queries.GetCommentsByUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Comment.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{CommentId}")]
        public async Task<IActionResult> GetCommentById(GetCommentByIdQueryRequest request)
        {
            GetCommentByIdQueryResponse response = await mediator.Send(request);
            return Ok(response);
        }
        [HttpGet("[action]/{UserId}")]
        public async Task<IActionResult> GetCommentsByUser([FromRoute]GetCommentsByUserQueryRequest request)
        {
            GetCommentsByUserQueryResponse response = await mediator.Send(request);
            return Ok(response);
        }
        [HttpGet("[action]/{PostId}")]
        public async Task<IActionResult> GetCommentsByPost([FromRoute]GetCommentsByPostQueryRequest request)
        {
            GetCommentsByPostQueryResponse response = await mediator.Send(request);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentCommandRequest request)
        {
            CreateCommentCommandResponse response = await mediator.Send(request);
            return Ok(response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteComment(DeleteCommentCommandRequest request)
        {
            DeleteCommentCommandResponse response = await mediator.Send(request);
            return Ok(response);
        }
    }
}
