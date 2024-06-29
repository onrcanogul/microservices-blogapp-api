using MassTransit;
using Microsoft.EntityFrameworkCore;
using Post.Service.Models.Contexts;
using Post.Service.Models.Entities;
using Shared;
using Shared.Events;
using System.Text.Json;

namespace Post.Service.Consumers
{
    public class CommentCreatedEventConsumer(PostDbContext dbContext, ISendEndpointProvider sendEndpointProvider) : IConsumer<CommentCreatedEvent>
    {
        public async Task Consume(ConsumeContext<CommentCreatedEvent> context)
        {
            //todo seperate to inbox service
            var result = await dbContext.CommentInboxes.AnyAsync(x => x.IdempotentToken == context.Message.IdempotentToken);
            if (!result)
            {
                CommentInbox commentInbox = new()
                {
                    IdempotentToken = context.Message.IdempotentToken,
                    Processed = false,
                    Payload = JsonSerializer.Serialize(context.Message)
                };

                await dbContext.CommentInboxes.AddAsync(commentInbox);
                await dbContext.SaveChangesAsync();
            }

            List<CommentInbox> commentInboxes = await dbContext.CommentInboxes
                .Where(x => x.Processed == false)
                .ToListAsync();

            foreach (var cInbox in commentInboxes)
            {
                CommentCreatedEvent? commentCreatedEvent = JsonSerializer.Deserialize<CommentCreatedEvent>(cInbox.Payload);

                Models.Entities.Post? post = await dbContext.Posts.FirstOrDefaultAsync(x => x.Id == commentCreatedEvent.PostId);
                if (post is not null)
                {
                        post.CommentsCount++;
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

                cInbox.Processed = true;
                await dbContext.SaveChangesAsync();
                
            }
        }
    }
}
