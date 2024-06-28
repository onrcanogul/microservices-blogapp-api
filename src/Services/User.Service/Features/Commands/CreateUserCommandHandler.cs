using MediatR;
using Microsoft.AspNetCore.Identity;
using User.Service.Models.Dtos;
using User.Service.Models.Entities;
using User.Service.Services.Abstractions;

namespace User.Service.Features.Commands
{
    public class CreateUserCommandHandler(IUserService userService) : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            bool result = await userService.CreateUserAsync(request.UserName, request.Email, request.Password, request.ConfirmPassword);
            return new()
            {
                IsSuccess = result,
            };
        }
    }
}
