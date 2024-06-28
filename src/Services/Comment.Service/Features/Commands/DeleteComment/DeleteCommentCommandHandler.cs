using Comment.Service.Services.Abstractions;
using MediatR;

namespace Comment.Service.Features.Commands.DeleteComment
{
    public class DeleteCommentCommandHandler(ICommentService service) : IRequestHandler<DeleteCommentCommandRequest, DeleteCommentCommandResponse>
    {
        public async Task<DeleteCommentCommandResponse> Handle(DeleteCommentCommandRequest request, CancellationToken cancellationToken)
        {
            bool result = await service.DeleteCommentAsync(request.CommentId);
            return new()
            {
                IsSuccess = result,
            };
        }   
    }
}
