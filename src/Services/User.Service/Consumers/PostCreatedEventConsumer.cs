using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Post.Service.Services.Abstractions;
using Shared;
using Shared.Events;
using User.Service.Models.Entities;

namespace User.Service.Consumers
{
    public class PostCreatedEventConsumer(ICommentInboxService<PostCreatedEvent,PostInbox> inboxService, UserManager<AppUser> userManager, ISendEndpointProvider sendEndpointProvider) : IConsumer<PostCreatedEvent>
    {
        public async Task Consume(ConsumeContext<PostCreatedEvent> context)
        {
            await inboxService.CreateAsync(context.Message.IdempotentToken,context.Message,Enums.EventType.Post);

            List<PostInbox> postInboxes = await inboxService.GetNotProcessedPostInboxes();
                
            foreach (var postInbox in postInboxes)
            {
                PostCreatedEvent postCreatedEvent = inboxService.GetEvent(postInbox.Payload);
                AppUser? user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == postCreatedEvent.UserId);
                if(user != null)
                    user.PostCount++;  
                else
                {
                    UserNotFoundEvent userNotFoundEvent = new()
                    {
                        CommentId = Guid.Empty,
                        PostId = postCreatedEvent.PostId,
                        UserId = postCreatedEvent.UserId
                    };
                    ISendEndpoint sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new($"queue:{RabbitMqSettings.Post_UserNotFoundEventQueue}"));
                    await sendEndpoint.Send(userNotFoundEvent);
                }
            }

        }
    }
}
