using Microsoft.EntityFrameworkCore;
using User.Service.Models.Contexts;
using User.Service.Models.Entities;
using Post.Service.Services.Abstractions;
using Shared.Base;
using System.Text.Json;
using User.Service.Enums;

namespace Post.Service.Services
{
    public class CommentInboxService<T, Inbox>(AppUserDbContext context) : ICommentInboxService<T, Inbox> where T : IEvent where Inbox : BaseInbox
    {
        public async Task CreateAsync(Guid IdempotentToken, object @event, EventType eventType)
        {
            bool result = true;
            if (eventType == EventType.Post)
            {
                result = await context.PostInboxes.AnyAsync(x => x.IdempotentToken == IdempotentToken);
                if (!result)
                {
                    PostInbox postInbox = new()
                    {
                        IdempotentToken = IdempotentToken,
                        Payload = JsonSerializer.Serialize(@event),
                        Processed = false
                    };
                    await context.PostInboxes.AddAsync(postInbox);
                }
            }
            else if (eventType == EventType.Comment) 
            {
                result = await context.CommentInboxes.AnyAsync(x => x.IdempotentToken == IdempotentToken);
                if (!result)
                {
                    CommentInbox commentInbox = new()
                    {
                        IdempotentToken = IdempotentToken,
                        Payload = JsonSerializer.Serialize(@event),
                        Processed = false
                    };
                    await context.CommentInboxes.AddAsync(commentInbox);
                }
               
            }
            await context.SaveChangesAsync();
        }

        public T GetEvent(string payload)
        {
            T @event = JsonSerializer.Deserialize<T>(payload)!;
            return @event;
        }

        public async Task<List<CommentInbox>> GetNotProcessedCommentInboxes()
        {
            List<CommentInbox> commentInboxes = await context.CommentInboxes
                .Where(x => x.Processed == false)
                .ToListAsync();
            return commentInboxes;                                                                       
        }
        public async Task<List<PostInbox>> GetNotProcessedPostInboxes()
        {
            List<PostInbox> PostInboxes = await context.PostInboxes
                .Where(x => x.Processed == false)
                .ToListAsync();
            return PostInboxes;
        }

        public async Task MakeProcessed(Inbox commentInbox)
        {
            commentInbox.Processed = true;
            await context.SaveChangesAsync();
        }

       

        
    }
}
