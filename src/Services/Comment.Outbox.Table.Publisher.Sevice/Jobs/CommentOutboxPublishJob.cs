using Comment.Outbox.Table.Publisher.Sevice.Entities;
using MassTransit;
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
    public class CommentOutboxPublishJob(IPublishEndpoint publishEndpoint) : IJob
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
                        //todo bütün eventlerle birlikte else yazdıktan sonra bu method dışarı alınababilir ve tek bir kere yazılabilir ama şimdi alınamaz.
                        await UpdateProcessedOn(commentOutbox);

                    }
                    else if(commentOutbox.Type == nameof(CommentDeletedEvent))
                    {
                        CommentDeletedEvent commentDeletedEvent = JsonSerializer.Deserialize<CommentDeletedEvent>(commentOutbox.Payload)!;
                        await publishEndpoint.Publish(commentDeletedEvent);
                        //todo bütün eventlerle birlikte else yazdıktan sonra bu method dışarı alınababilir ve tek bir kere yazılabilir ama şimdi alınamaz.
                        await UpdateProcessedOn(commentOutbox);

                    }
                }
                CommentOutboxSingletonDatabase.DataReaderReady();
                Console.WriteLine("comment outbox checked");
            }
        }
        private async Task UpdateProcessedOn(CommentOutbox commentOutbox)
        {
            await CommentOutboxSingletonDatabase.ExecuteAsync($"UPDATE COMMENTOUTBOXES SET PROCESSEDON = GETDATE() WHERE IdempotentToken = '{commentOutbox.IdempotentToken}'");
        }
    }

    
}
