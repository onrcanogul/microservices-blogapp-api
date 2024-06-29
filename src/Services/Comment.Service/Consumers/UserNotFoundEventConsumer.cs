using Comment.Service.Exceptions;
using Comment.Service.Services;
using Comment.Service.Services.Abstractions;
using MassTransit;
using MongoDB.Driver;
using Shared.Events;

namespace Comment.Service.Consumers
{
    public class UserNotFoundEventConsumer(ICommentService commentService) : IConsumer<UserNotFoundEvent>
    {
        public async Task Consume(ConsumeContext<UserNotFoundEvent> context)
        => await commentService.DeleteCommentAsync(context.Message.CommentId.ToString());
        
    }
}
