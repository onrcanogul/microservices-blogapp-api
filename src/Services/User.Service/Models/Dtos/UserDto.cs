namespace User.Service.Models.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int PostCount { get; set; }
        public int CommentCount { get; set; }
    }
}
