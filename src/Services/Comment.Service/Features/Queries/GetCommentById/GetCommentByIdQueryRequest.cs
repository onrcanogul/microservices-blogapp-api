using MediatR;

namespace Comment.Service.Features.Queries.GetCommentById
{
    public class GetCommentByIdQueryRequest : IRequest<GetCommentByIdQueryResponse>
    {
        public string CommentId { get; set; } = null!;
    }
}