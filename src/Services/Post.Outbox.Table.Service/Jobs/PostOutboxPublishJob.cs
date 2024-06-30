
using MassTransit;
using Post.Outbox.Table.Service;
using Post.Outbox.Table.Services;
using Quartz;
using Shared.Events;
using System.Text.Json;

namespace Post.Outbox.Table.Sevice.Jobs
{
    public class PostOutboxPublishJob(IPublishEndpoint publishEndpoint) : IJob
    {
        
        public async Task Execute(IJobExecutionContext context)
        {
            if (PostOutboxSingletonDatabase.DataReaderState)
            {
                PostOutboxSingletonDatabase.DataReaderBusy();
                List<PostOutbox> postOutboxes = (await PostOutboxSingletonDatabase.QueryAsync<PostOutbox>("SELECT * FROM POSTOUTBOXES WHERE PROCESSEDON IS NULL ORDER BY OCCUREDON ASC")).ToList();

                foreach (var PostOutbox in postOutboxes)
                {
                    if(PostOutbox.Type == nameof(PostCreatedEvent))
                    { 
                        PostCreatedEvent postCreatedEvent = JsonSerializer.Deserialize<PostCreatedEvent>(PostOutbox.Payload)!;
                        await publishEndpoint.Publish(postCreatedEvent);
                        
                    }
                    else if(PostOutbox.Type == nameof(PostDeletedEvent))
                    {
                        PostDeletedEvent postDeletedEvent = JsonSerializer.Deserialize<PostDeletedEvent>(PostOutbox.Payload)!;
                        await publishEndpoint.Publish(postDeletedEvent);
                       

                    }
                    else          
                        throw new EventNotExistException(PostOutbox.Type);
                    await UpdateProcessedOn(PostOutbox);

                }
                PostOutboxSingletonDatabase.DataReaderReady();
            }
        }
        private async Task UpdateProcessedOn(PostOutbox outbox)
        {
            await PostOutboxSingletonDatabase.ExecuteAsync($"UPDATE POSTOUTBOXES SET PROCESSEDON = GETDATE() WHERE IdempotentToken = '{outbox.IdempotentToken}'");
        }
    }

    
}
