using Comment.Service.Services.Abstractions;
using MediatR;

namespace Comment.Service.Features.Commands.CreateComment
{
    public class CreateCommentCommandHandler(ICommentService service) : IRequestHandler<CreateCommentCommandRequest, CreateCommentCommandResponse>
    {
        public async Task<CreateCommentCommandResponse> Handle(CreateCommentCommandRequest request, CancellationToken cancellationToken)
        {
            bool result = await service.CreateCommentAsync(request.Message, request.UserId, request.PostId);
            return new()
            {
                IsSuccess = result,
            };
        }
    }
}
