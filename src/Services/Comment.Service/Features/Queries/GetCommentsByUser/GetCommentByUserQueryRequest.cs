using MediatR;

namespace Comment.Service.Features.Queries.GetCommentsByUser
{
    public class GetCommentsByUserQueryRequest : IRequest<GetCommentsByUserQueryResponse>
    {
        public string UserId { get; set; } = null!;
    }
}