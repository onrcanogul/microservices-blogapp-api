using MassTransit;
using Post.Service.Models.Entities;
using Shared.Base;

namespace Post.Service.Services.Abstractions
{
    public interface IInboxService<T> where T:IEvent
    {
        Task CreateAsync(Guid IdempotentToken, object @event);

        Task<List<CommentInbox>> GetNotProcessedInboxes();

        Task MakeProcessed(CommentInbox commentInbox);

        T GetEvent(string payload);
    }
}
