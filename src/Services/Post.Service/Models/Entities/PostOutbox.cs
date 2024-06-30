using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Post.Service.Models.Entities
{
    public class PostOutbox
    {
        [Key]
        public Guid IdempotentToken { get; set; }
        public DateTime OccuredOn { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
    }
}
