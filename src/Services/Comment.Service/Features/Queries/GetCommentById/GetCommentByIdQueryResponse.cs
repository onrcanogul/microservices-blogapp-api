using Comment.Service.Models.Dtos;

namespace Comment.Service.Features.Queries.GetCommentById
{
    public class GetCommentByIdQueryResponse
    {
        public CommentDto Comment { get; set; }
    }
}