using MediatR;

namespace Post.Service.Features.Queries.GetPosts
{
    public class GetPostsQueryRequest : IRequest<GetPostsQueryResponse>
    {
    }
}