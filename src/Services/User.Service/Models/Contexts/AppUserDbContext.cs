using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using User.Service.Models.Entities;

namespace User.Service.Models.Contexts
{
    public class AppUserDbContext : IdentityDbContext<AppUser,IdentityRole,string>
    {
        public AppUserDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<CommentInbox> CommentInboxes { get; set; }
        public DbSet<PostInbox> PostInboxes { get; set; }
    }
}
