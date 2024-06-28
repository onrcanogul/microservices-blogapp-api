using MediatR;
using User.Service.Models.Dtos;
using User.Service.Services.Abstractions;

namespace User.Service.Features.Queries.GetUserById
{
    public class GetUserByIdQueryHandler(IUserService userService) : IRequestHandler<GetUserByIdQueryRequest, GetUserByIdQueryResponse>
    {
        public async Task<GetUserByIdQueryResponse> Handle(GetUserByIdQueryRequest request, CancellationToken cancellationToken)
        {
            UserDto user = await userService.GetUserById(request.UserId);
            return new()
            {
                User = user
            };
        }
    }
}
