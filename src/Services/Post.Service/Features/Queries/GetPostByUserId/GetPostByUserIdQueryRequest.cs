using MediatR;

namespace Post.Service.Features.Queries.GetPostByUserId
{
    public class GetPostByUserIdQueryRequest : IRequest<GetPostByUserIdQueryResponse>
    {
        public string UserId { get; set; } = null!;
    }
}