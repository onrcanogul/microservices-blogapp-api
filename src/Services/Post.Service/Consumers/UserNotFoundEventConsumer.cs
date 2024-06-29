using MassTransit;
using Microsoft.EntityFrameworkCore;
using Post.Service.Models.Contexts;
using Shared.Events;

namespace Post.Service.Consumers
{
    public class UserNotFoundEventConsumer(PostDbContext dbContext) : IConsumer<UserNotFoundEvent>
    {
        public async Task Consume(ConsumeContext<UserNotFoundEvent> context)
        {
            Models.Entities.Post? post = await dbContext.Posts.FirstOrDefaultAsync(x => x.Id == context.Message.PostId);
            if(post is not null)
            {
                post.CommentsCount--;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
