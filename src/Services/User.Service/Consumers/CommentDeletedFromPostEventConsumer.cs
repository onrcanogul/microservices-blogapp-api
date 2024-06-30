using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Post.Service.Services.Abstractions;
using Shared.Events;
using User.Service.Models.Dtos;
using User.Service.Models.Entities;
using User.Service.Services.Abstractions;

namespace User.Service.Consumers
{
    public class CommentDeletedFromPostEventConsumer(ICommentInboxService<CommentDeletedFromPostEvent,CommentInbox> commentInboxService, UserManager<AppUser> userManager, IPublishEndpoint publishEndpoint) : IConsumer<CommentDeletedFromPostEvent>
    {
        public async Task Consume(ConsumeContext<CommentDeletedFromPostEvent> context)
        {
            await commentInboxService.CreateAsync(context.Message.IdempotentToken, context.Message, Enums.EventType.Comment);
            List<CommentInbox> commentInboxes = await commentInboxService.GetNotProcessedCommentInboxes();

            foreach (var commentInbox in commentInboxes)
            {
                CommentDeletedFromPostEvent commentDeletedFromPostEvent = commentInboxService.GetEvent(commentInbox.Payload);
                AppUser? user = await userManager.Users.FirstOrDefaultAsync(x => x.Id ==  commentDeletedFromPostEvent.UserId);
                if(user != null)
                {
                    user.CommentCount--;
                    await commentInboxService.MakeProcessed(commentInbox);
                }
                else
                {
                    UserNotFoundEvent userNotFoundEvent = new()
                    {
                        CommentId = commentDeletedFromPostEvent.CommentId,
                        UserId = commentDeletedFromPostEvent.UserId,
                        PostId = commentDeletedFromPostEvent.PostId
                    };
                    await publishEndpoint.Publish(userNotFoundEvent);
                }
            }
        }

    }
}
