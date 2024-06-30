using Comment.Outbox.Table.Publisher.Sevice;
using Comment.Outbox.Table.Publisher.Sevice.Entities;
using MassTransit;
using MassTransit.Transports;
using Outbox.Tables.Sevice.Entities;
using Quartz;
using Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Outbox.Tables.Sevice.Jobs
{
    public class PostOutboxPublishJob(IPublishEndpoint publishEndpoint, IConfiguration configuration) : IJob
    {
        OutboxDatabase commentOutboxDatabase = new(configuration.GetConnectionString("PostOutbox")!);
        public async Task Execute(IJobExecutionContext context)
        {
            if (commentOutboxDatabase.DataReaderState)
            {
                commentOutboxDatabase.DataReaderBusy();
                List<PostOutbox> postOutboxes = (await commentOutboxDatabase.QueryAsync<PostOutbox>("SELECT * FROM POSTOUTBOXES WHERE PROCESSEDON IS NULL ORDER BY OCCUREDON ASC")).ToList();

                foreach (var postOutbox in postOutboxes)
                {
                    if (postOutbox.Type == nameof(PostCreatedEvent))
                    {
                        PostCreatedEvent commentCreatedEvent = JsonSerializer.Deserialize<PostCreatedEvent>(postOutbox.Payload)!;
                        await publishEndpoint.Publish(commentCreatedEvent);
                        //todo bütün eventlerle birlikte else yazdıktan sonra bu method dışarı alınababilir ve tek bir kere yazılabilir ama şimdi alınamaz.
                        await UpdateProcessedOn(postOutbox);

                    }
                    else if (postOutbox.Type == nameof(PostDeletedEvent))
                    {
                        PostDeletedEvent commentDeletedEvent = JsonSerializer.Deserialize<PostDeletedEvent>(postOutbox.Payload)!;
                        await publishEndpoint.Publish(commentDeletedEvent);
                        await UpdateProcessedOn(postOutbox);

                    }
                }
                commentOutboxDatabase.DataReaderReady();
                Console.WriteLine("comment outbox checked");
            }
        }
        private async Task UpdateProcessedOn(BaseOutbox outbox)
        {
            await commentOutboxDatabase.ExecuteAsync($"UPDATE COMMENTOUTBOXES SET PROCESSEDON = GETDATE() WHERE IdempotentToken = '{outbox.IdempotentToken}'");
        }
    }
}

