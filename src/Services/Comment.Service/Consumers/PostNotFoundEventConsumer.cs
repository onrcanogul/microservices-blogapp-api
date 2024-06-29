using Comment.Service.Exceptions;
using Comment.Service.Services.Abstractions;
using MassTransit;
using MongoDB.Driver;
using Shared.Events;

namespace Comment.Service.Consumers
{
    public class PostNotFoundEventConsumer(ICommentService commentService) : IConsumer<PostNotFoundEvent>
    {
        public async Task Consume(ConsumeContext<PostNotFoundEvent> context)
        => await commentService.DeleteCommentAsync(context.Message.CommentId.ToString());
    }
}
