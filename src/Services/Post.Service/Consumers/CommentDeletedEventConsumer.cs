using MassTransit;
using Microsoft.EntityFrameworkCore;
using Post.Service.Models.Contexts;
using Post.Service.Models.Entities;
using Post.Service.Services.Abstractions;
using Shared;
using Shared.Events;

namespace Post.Service.Consumers
{
    public class CommentDeletedEventConsumer(PostDbContext dbContext, ISendEndpointProvider sendEndpointProvider, ICommentInboxService<CommentDeletedEvent> commentInboxService) : IConsumer<CommentDeletedEvent>
    {
        public async Task Consume(ConsumeContext<CommentDeletedEvent> context)
        {
            await commentInboxService.CreateAsync(context.Message.IdempotentToken, context.Message);
            List<CommentInbox> commentInboxes = await commentInboxService.GetNotProcessedInboxes();

            foreach (var cInbox in commentInboxes)
            {
                CommentDeletedEvent commentDeletedEvent = commentInboxService.GetEvent(cInbox.Payload);

                Models.Entities.Post? post = await dbContext.Posts.Where(x => x.Id == commentDeletedEvent.PostId).FirstOrDefaultAsync();
                if(post is not null)
                {
                    post.CommentsCount--;
                    CommentDeletedFromPostEvent commentDeletedFromPostEvent = new()
                    {
                        CommentId = commentDeletedEvent.CommentId,
                        UserId = commentDeletedEvent.UserId,
                        IdempotentToken = commentDeletedEvent.IdempotentToken,
                        PostId = commentDeletedEvent.PostId
                    };
                    ISendEndpoint sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new($"queue:{RabbitMqSettings.User_CommentDeletedFromPostEventQueue}"));
                    Task sendEvent = sendEndpoint.Send(commentDeletedFromPostEvent);

                    cInbox.Processed = true;
                    Task save = dbContext.SaveChangesAsync();

                    await Task.WhenAll(save, sendEvent);
                }
                else
                {
                    PostNotFoundEvent postNotFoundEvent = new()
                    {
                        CommentId = commentDeletedEvent.CommentId,
                        PostId = commentDeletedEvent.PostId
                    };
                    ISendEndpoint sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new($"queue:{RabbitMqSettings.Comment_PostNotFoundEventQueue}"));
                    await sendEndpoint.Send(postNotFoundEvent);
                }
                
            }
        }
    }
}
