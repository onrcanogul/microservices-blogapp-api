using MassTransit;
using Microsoft.AspNetCore.Identity;
using Post.Service.Services.Abstractions;
using Shared;
using Shared.Events;
using User.Service.Models.Entities;

namespace User.Service.Consumers
{
    public class PostDeletedEventConsumer(ICommentInboxService<PostDeletedEvent,PostInbox> inboxService, UserManager<AppUser> userManager, ISendEndpointProvider sendEndpointProvider) : IConsumer<PostDeletedEvent>
    {
        public async Task Consume(ConsumeContext<PostDeletedEvent> context)
        {
            await inboxService.CreateAsync(context.Message.IdempotentToken, context.Message);

            List<PostInbox> postInboxes = await inboxService.GetNotProcessedInboxes();
            foreach (var postInbox in postInboxes) 
            {
                PostDeletedEvent postDeletedEvent = inboxService.GetEvent(postInbox.Payload);
                AppUser? user = await userManager.FindByIdAsync(postDeletedEvent.UserId);
                if (user != null) 
                {
                    user.PostCount--;
                }
                else
                {
                    UserNotFoundEvent userNotFoundEvent = new()
                    {
                        CommentId = Guid.Empty,
                        PostId = postDeletedEvent.PostId,
                        UserId = postDeletedEvent.UserId,
                    };
                    ISendEndpoint sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new($"queue:{RabbitMqSettings.Post_UserNotFoundEventQueue}"));
                    await sendEndpoint.Send(userNotFoundEvent);
                }
            }

        }
    }
}
