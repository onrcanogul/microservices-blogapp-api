﻿using Shared.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class CommentDeletedEvent : IEvent
    {
        public Guid CommentId { get; set; }
        public Guid PostId { get; set; }
        public string UserId { get; set; }
        public Guid IdempotentToken { get; set; }
    }
}
