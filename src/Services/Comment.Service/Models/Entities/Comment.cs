using Shared.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Comment.Service.Models.Entities
{
    public class Comment : BaseEntity
    {
        public Guid PostId { get; set; }
        public string UserId { get; set; } = null!;
        public string Message { get; set; } = null!;
        [NotMapped]
        public override DateTime UpdatedDate { get => base.UpdatedDate; set => base.UpdatedDate = value; }
    }
}
