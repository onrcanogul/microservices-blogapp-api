using MediatR;
using User.Service.Models.Dtos;
using User.Service.Services.Abstractions;

namespace User.Service.Features.Queries.GetUsers
{
    public class GetUsersQueryHandler(IUserService userService) : IRequestHandler<GetUsersQueryRequest, GetUsersQueryResponse>
    {
        public async Task<GetUsersQueryResponse> Handle(GetUsersQueryRequest request, CancellationToken cancellationToken)
        {
            List<UserDto> users = await userService.GetUsers();
            return new()
            {
                Users = users,
            };
        }
    }
}
