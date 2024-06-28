using Post.Service.Models.Dtos;

namespace Post.Service.Features.Queries.GetPostByUserId
{
    public class GetPostByUserIdQueryResponse
    {
        public List<PostDto> Posts { get; set; } = null!;
    }
}