using MediatR;

namespace Post.Service.Features.Queries.GetPostById
{
    public class GetPostByIdQueryRequest : IRequest<GetPostByIdQueryResponse>
    {
        public string PostId { get; set; } = null!;
    }
}