using MassTransit;
using User.Service.Models.Entities;
using Shared.Base;
using User.Service.Enums;

namespace Post.Service.Services.Abstractions
{
    public interface ICommentInboxService<T,Inbox> where T:IEvent where Inbox : BaseInbox
    {
        Task CreateAsync(Guid IdempotentToken, object @event, EventType eventType);
        Task<List<CommentInbox>> GetNotProcessedCommentInboxes();
        Task MakeProcessed(Inbox commentInbox);
        T GetEvent(string payload);
        Task<List<PostInbox>> GetNotProcessedPostInboxes();
    }
}
