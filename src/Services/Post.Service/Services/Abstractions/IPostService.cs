using Post.Service.Models.Dtos;

namespace Post.Service.Services.Abstractions
{
    public interface IPostService
    {
        Task<bool> CreatePostAsync(string title, string description, string userId);
        Task<bool> RemovePostAsync(string postId);
        Task UpdatePostAsync(string postId, string title, string description);
        Task<List<PostDto>> GetPostsAsync();
        Task<PostDto> GetPostByIdAsync(string postId);
        Task<List<PostDto>> GetPostByUserIdAsync(string userId);
    }
}
