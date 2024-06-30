using MassTransit;
using User.Service.Models.Entities;
using Shared.Base;

namespace Post.Service.Services.Abstractions
{
    public interface ICommentInboxService<T,Inbox> where T:IEvent where Inbox : BaseInbox
    {
        Task CreateAsync(Guid IdempotentToken, object @event);

        Task<List<Inbox>> GetNotProcessedInboxes();

        Task MakeProcessed(Inbox commentInbox);

        T GetEvent(string payload);
    }
}
