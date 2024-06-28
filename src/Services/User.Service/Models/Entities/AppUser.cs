using Microsoft.AspNetCore.Identity;

namespace User.Service.Models.Entities
{
    public class AppUser : IdentityUser<string>
    {
        public int PostCount { get; set; }
        public int CommentCount { get; set; }
    }
}
