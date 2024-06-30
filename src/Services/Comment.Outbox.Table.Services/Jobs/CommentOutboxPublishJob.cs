using Comment.Outbox.Table.Services;
using Comment.Outbox.Table.Services.Entities;
using MassTransit;
using Quartz;
using Shared.Events;
using System.Text.Json;

namespace Comment.Outbox.Table.Sevice.Jobs
{
    public class CommentOutboxPublishJob(IPublishEndpoint publishEndpoint ,IConfiguration configuration) : IJob
    {
        
        public async Task Execute(IJobExecutionContext context)
        {
            if (CommentOutboxSingletonDatabase.DataReaderState)
            {
                CommentOutboxSingletonDatabase.DataReaderBusy();
                List<CommentOutbox> commentOutboxes = (await CommentOutboxSingletonDatabase.QueryAsync<CommentOutbox>("SELECT * FROM COMMENTOUTBOXES WHERE PROCESSEDON IS NULL ORDER BY OCCUREDON ASC")).ToList();

                foreach (var commentOutbox in commentOutboxes)
                {
                    if(commentOutbox.Type == nameof(CommentCreatedEvent))
                    { 
                        CommentCreatedEvent commentCreatedEvent = JsonSerializer.Deserialize<CommentCreatedEvent>(commentOutbox.Payload)!;
                        await publishEndpoint.Publish(commentCreatedEvent);
                        

                    }
                    else if(commentOutbox.Type == nameof(CommentDeletedEvent))
                    {
                        CommentDeletedEvent commentDeletedEvent = JsonSerializer.Deserialize<CommentDeletedEvent>(commentOutbox.Payload)!;
                        await publishEndpoint.Publish(commentDeletedEvent);

                    }
                    else          
                        throw new EventNotExistException(commentOutbox.Type);
                    await UpdateProcessedOn(commentOutbox);

                }
                CommentOutboxSingletonDatabase.DataReaderReady();
            }
        }
        private async Task UpdateProcessedOn(CommentOutbox outbox)
        {
            await CommentOutboxSingletonDatabase.ExecuteAsync($"UPDATE COMMENTOUTBOXES SET PROCESSEDON = GETDATE() WHERE IdempotentToken = '{outbox.IdempotentToken}'");
        }
    }

    
}
