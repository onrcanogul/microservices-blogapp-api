using System.ComponentModel.DataAnnotations;

namespace User.Service.Models.Entities
{
    public class BaseInbox
    {
        [Key]
        public Guid IdempotentToken { get; set; }
        public string Payload { get; set; }
        public bool Processed { get; set; }
    }
}
