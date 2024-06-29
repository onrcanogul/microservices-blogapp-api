using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Post.Service.Services.Abstractions;
using Shared.Events;
using User.Service.Models.Entities;

namespace User.Service.Consumers
{
    public class CommentSavedToPostEventConsumer(ICommentInboxService<CommentSavedToPostEvent> commentInboxService, UserManager<AppUser> userManager,IPublishEndpoint publishEndpoint) : IConsumer<CommentSavedToPostEvent>
    {
        public async Task Consume(ConsumeContext<CommentSavedToPostEvent> context)
        {
            await commentInboxService.CreateAsync(context.Message.IdempotentToken, context.Message);
            List<CommentInbox> commentInboxes = await commentInboxService.GetNotProcessedInboxes();

            foreach (var commentInbox in commentInboxes)
            {
                CommentSavedToPostEvent commentSavedToPostEvent = commentInboxService.GetEvent(commentInbox.Payload);
                AppUser? user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == commentSavedToPostEvent.UserId);
                if(user != null)
                {
                    user.CommentCount++;
                    await commentInboxService.MakeProcessed(commentInbox);
                }
                else
                {
                    UserNotFoundEvent userNotFoundEvent = new()
                    {
                        CommentId = commentSavedToPostEvent.CommentId,
                        UserId = commentSavedToPostEvent.UserId,
                    };
                    await publishEndpoint.Publish(userNotFoundEvent);
                }
                         
            }
        }
    }
}
