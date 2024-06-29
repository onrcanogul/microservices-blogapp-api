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
                List<CommentOutbox> commentOutboxes = (await CommentOutboxSingletonDatabase.QueryAsync<CommentOutbox>("SELECT * FROM COMMENTOUTBOXES WHERE PROCESSED ON IS NULL ORDER BY OCURREDON ASC")).ToList();

                foreach (var commentOutbox in commentOutboxes)
                {
                    CommentCreatedEvent commentCreatedEvent = JsonSerializer.Deserialize<CommentCreatedEvent>(commentOutbox.Payload);

                    if (commentCreatedEvent is not null)
                    {
                        await publishEndpoint.Publish(commentCreatedEvent);
                        await CommentOutboxSingletonDatabase.ExecuteAsync($"UPDATE COMMENTOUTBOXES SET PROCESSEDON = GETDATE() WHERE IdempotentToken = '{commentOutbox.IdempotentToken}'");
                    }
                }
                CommentOutboxSingletonDatabase.DataReaderReady();
                Console.WriteLine("comment outbox checked");
            }
        }
    }
}
