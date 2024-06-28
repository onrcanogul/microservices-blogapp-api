using Comment.Service.Models.Dtos;

namespace Comment.Service.Services.Abstractions
{
    public interface ICommentService
    {
        Task<CommentDto> GetCommentByIdAsync(string commentId);
        Task<List<CommentDto>> GetCommentsByPostAsync(string postId);
        Task<List<CommentDto>> GetCommentByUserAsync(string userId);
        Task<bool> CreateCommentAsync(string message, string userId, string postId);
        Task<bool> DeleteCommentAsync(string commentId);
    }
}
