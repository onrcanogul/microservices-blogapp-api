using MassTransit.Saga;
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
        public const string User_CommentDeletedEventQueue = "user-comment-deleted-event-queue";
        public const string Post_CommentDeletedEventQueue = "post-comment-deleted-event-queue";
        public const string User_CommentSavedToPostEventQueue = "user-comment-saved-to-post-event-queue";
        public const string User_CommentDeletedFromPostEventQueue = "user-comment-deleted-from-post-event-queue";
        public const string Post_UserNotFoundEventQueue = "post-user-not-found-event-queue";
        public const string Comment_UserNotFoundEventQueue = "comment-user-not-found-event-queue";
        public const string User_PostCreatedEventQueue = "user-post-created-event-queue";
        public const string User_PostDeletedEventQueue = "user-post-deleted-event-queue";
    }
}
