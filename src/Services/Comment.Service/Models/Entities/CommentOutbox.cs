using System.ComponentModel.DataAnnotations;

namespace Comment.Service.Models.Entities
{
    public class CommentOutbox
    {
        [Key]
        public Guid IdempotentToken { get; set; }
        public DateTime OccuredOn { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
    }
}
