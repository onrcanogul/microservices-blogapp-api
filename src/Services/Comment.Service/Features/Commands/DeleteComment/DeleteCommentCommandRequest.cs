using MediatR;

namespace Comment.Service.Features.Commands.DeleteComment
{
    public class DeleteCommentCommandRequest : IRequest<DeleteCommentCommandResponse>
    {
        public string CommentId { get; set; } = null!;
    }
}