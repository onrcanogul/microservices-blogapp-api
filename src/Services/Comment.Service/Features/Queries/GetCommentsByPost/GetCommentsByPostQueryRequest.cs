using MediatR;

namespace Comment.Service.Features.Queries.GetCommentsByPost
{
    public class GetCommentsByPostQueryRequest : IRequest<GetCommentsByPostQueryResponse>
    {
        public string PostId { get; set; }
    }
}