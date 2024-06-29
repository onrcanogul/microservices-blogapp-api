using Comment.Service.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Comment.Service.Models.Contexts
{
    public class CommentOutboxDbContext : DbContext
    {
        public CommentOutboxDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<CommentOutbox> CommentOutboxes { get; set; }
    }
}
