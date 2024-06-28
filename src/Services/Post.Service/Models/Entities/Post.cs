using Shared.Base;

namespace Post.Service.Models.Entities
{
    public class Post : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int CommentsCount { get; set; }
        public string UserId { get; set; } = null!;

    }
}
