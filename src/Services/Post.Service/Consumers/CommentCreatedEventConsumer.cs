using MassTransit;
using Microsoft.EntityFrameworkCore;
using Post.Service.Models.Contexts;
using Post.Service.Models.Entities;
using Post.Service.Services.Abstractions;
using Shared;
using Shared.Events;


namespace Post.Service.Consumers
{
    public class CommentCreatedEventConsumer(PostDbContext dbContext, ISendEndpointProvider sendEndpointProvider, ICommentInboxService<CommentCreatedEvent> commentInboxService) : IConsumer<CommentCreatedEvent>
    {
        public async Task Consume(ConsumeContext<CommentCreatedEvent> context)
        {   
            await commentInboxService.CreateAsync(context.Message.IdempotentToken, context.Message);
            List<CommentInbox> commentInboxes = await commentInboxService.GetNotProcessedInboxes();

            foreach (var cInbox in commentInboxes)
            {
                CommentCreatedEvent commentCreatedEvent = commentInboxService.GetEvent(cInbox.Payload);

                Models.Entities.Post? post = await dbContext.Posts.FirstOrDefaultAsync(x => x.Id == commentCreatedEvent.PostId);
                if (post is not null)
                {
                    post.CommentsCount++;
                    CommentSavedToPostEvent commentSavedToPostEvent = new()
                    {
                        CommentId = commentCreatedEvent.CommentId,
                        UserId = commentCreatedEvent.UserId,
                        IdempotentToken = commentCreatedEvent.IdempotentToken,
                        PostId = commentCreatedEvent.PostId
                    };
                    ISendEndpoint sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new($"queue:{RabbitMqSettings.User_CommentSavedToPostEventQueue}"));

                    cInbox.Processed = true;
                    Task sendEvent = sendEndpoint.Send(commentSavedToPostEvent);
                    Task save = dbContext.SaveChangesAsync();

                    await Task.WhenAll(save, sendEvent);
                }  
                else
                {
                    PostNotFoundEvent postNotFoundEvent = new()
                    {
                        CommentId = commentCreatedEvent.CommentId,
                        PostId = commentCreatedEvent.PostId
                    };
                    ISendEndpoint sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new($"queue:{RabbitMqSettings.Comment_PostNotFoundEventQueue}"));
                    await sendEndpoint.Send(postNotFoundEvent);
                }

            }
        }
    }
}
