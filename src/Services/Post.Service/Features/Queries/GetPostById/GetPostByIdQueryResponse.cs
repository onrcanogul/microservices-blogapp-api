using Post.Service.Models.Dtos;

namespace Post.Service.Features.Queries.GetPostById
{
    public class GetPostByIdQueryResponse
    {
        public PostDto Post { get; set; } = null!;
    }
}