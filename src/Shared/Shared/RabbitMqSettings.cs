using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class RabbitMqSettings
    {
        public const string Post_CommentCreatedEventQueue = "post-comment-created-event-queue";
        public const string User_CommentCreatedEventQueue = "user-comment-created-event-queue";
        public const string Comment_PostNotFoundEventQueue = "comment-post-not-found-event-queue";

    }
}
