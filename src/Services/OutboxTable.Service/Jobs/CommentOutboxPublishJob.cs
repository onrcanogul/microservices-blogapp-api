using Comment.Outbox.Table.Publisher.Sevice.Entities;
using MassTransit;
using Outbox.Tables.Sevice.Entities;
using Outbox.Tables.Sevice.Exceptions;
using Quartz;
using Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Comment.Outbox.Table.Publisher.Sevice.Jobs
{
    public class CommentOutboxPublishJob(IPublishEndpoint publishEndpoint ,IConfiguration configuration) : IJob
    {
        OutboxDatabase commentOutboxSingleton = new(configuration.GetConnectionString("CommentOutbox")!);
        public async Task Execute(IJobExecutionContext context)
        {
            if (commentOutboxSingleton.DataReaderState)
            {
                commentOutboxSingleton.DataReaderBusy();
                List<CommentOutbox> commentOutboxes = (await commentOutboxSingleton.QueryAsync<CommentOutbox>("SELECT * FROM COMMENTOUTBOXES WHERE PROCESSEDON IS NULL ORDER BY OCCUREDON ASC")).ToList();

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
                commentOutboxSingleton.DataReaderReady();
            }
        }
        private async Task UpdateProcessedOn(BaseOutbox outbox)
        {
            await commentOutboxSingleton.ExecuteAsync($"UPDATE COMMENTOUTBOXES SET PROCESSEDON = GETDATE() WHERE IdempotentToken = '{outbox.IdempotentToken}'");
        }
    }

    
}
