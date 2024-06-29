using System.ComponentModel.DataAnnotations;

namespace Post.Service.Models.Entities
{
    public class CommentInbox
    {
        [Key]
        public Guid IdempotentToken { get; set; }
        public string Payload { get; set; }
        public bool Processed { get; set; }

    }
}
