using Shared.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class PostNotFoundEvent : IEvent
    {
        public Guid PostId { get; set; }
        public Guid CommentId { get; set; }
    }
}
