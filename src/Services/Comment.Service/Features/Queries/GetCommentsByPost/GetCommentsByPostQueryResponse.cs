using Comment.Service.Models.Dtos;

namespace Comment.Service.Features.Queries.GetCommentsByPost
{
    public class GetCommentsByPostQueryResponse
    {
        public List<CommentDto> Comments { get; set; }
    }
}