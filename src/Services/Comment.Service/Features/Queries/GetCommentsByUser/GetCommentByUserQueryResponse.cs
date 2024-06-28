using Comment.Service.Models.Dtos;

namespace Comment.Service.Features.Queries.GetCommentsByUser
{
    public class GetCommentsByUserQueryResponse
    {
        public List<CommentDto> Comments { get; set; } = null!;
    }
}