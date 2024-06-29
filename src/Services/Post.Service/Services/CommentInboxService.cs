using Microsoft.EntityFrameworkCore;
using Post.Service.Models.Contexts;
using Post.Service.Models.Entities;
using Post.Service.Services.Abstractions;
using Shared.Base;
using System.Text.Json;

namespace Post.Service.Services
{
    public class CommentInboxService<T>(PostDbContext context) : ICommentInboxService<T> where T: IEvent
    {
        public async Task CreateAsync(Guid IdempotentToken, object @event)
        {
            bool result = await context.CommentInboxes.AnyAsync(x => x.IdempotentToken == IdempotentToken);
            if (!result)
            {
                CommentInbox commentInbox = new()
                {
                    IdempotentToken = IdempotentToken,
                    Payload = JsonSerializer.Serialize(@event),
                    Processed = false
                };
            }
        }

        public T GetEvent(string payload)
        {
            T @event = JsonSerializer.Deserialize<T>(payload)!;
            return @event;
        }

        public async Task<List<CommentInbox>> GetNotProcessedInboxes()
        {
            List<CommentInbox> commentInboxes = await context.CommentInboxes
                .Where(x => x.Processed == false)
                .ToListAsync();
            return commentInboxes;                                                                       
        }

        public async Task MakeProcessed(CommentInbox commentInbox)
        {
            commentInbox.Processed = true;
            await context.SaveChangesAsync();
        }
    }
}
