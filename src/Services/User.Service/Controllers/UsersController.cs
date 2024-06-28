using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Service.Features.Commands;
using User.Service.Features.Queries.GetUserById;
using User.Service.Features.Queries.GetUsers;

namespace User.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest request)
        {
            CreateUserCommandResponse response = await mediator.Send(request);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            GetUsersQueryResponse response = await mediator.Send(new GetUsersQueryRequest());
            return Ok(response);
        }
        [HttpGet("{UserId}")]
        public async Task<IActionResult> GetUserById([FromRoute]GetUserByIdQueryRequest request)
        {
            GetUserByIdQueryResponse response = await mediator.Send(request);
            return Ok(response);
        }
    }
}
