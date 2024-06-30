using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Post.Service.Models.Contexts;
using Post.Service.Models.Entities;
using Shared.Events;
using System.Text.Json;

namespace Post.Service.Features.Commands.CreatePost
{
    public class CreatePostCommandHandler(PostDbContext context) : IRequestHandler<CreatePostCommandRequest, CreatePostCommandResponse>
    {
        public async Task<CreatePostCommandResponse> Handle(CreatePostCommandRequest request, CancellationToken cancellationToken)
        {
            Models.Entities.Post post = new()
            {
                CommentsCount = 0,
                Description = request.Description,
                Title = request.Title,
                UserId = request.UserId,
            };

            EntityEntry entityEntry = await context.Posts.AddAsync(post);

            Guid idempotentToken = Guid.NewGuid();
            PostCreatedEvent postCreatedEvent = new()
            {
                IdempotentToken = idempotentToken,
                PostId = post.Id,
                UserId = post.UserId
            };
            PostOutbox postOutbox = new()
            {
                IdempotentToken = idempotentToken,
                OccuredOn = DateTime.UtcNow,
                Payload = JsonSerializer.Serialize(postCreatedEvent),
                Type = postCreatedEvent.GetType().Name,
                ProcessedOn = null
            };
            await context.PostOutboxes.AddAsync(postOutbox);
            await context.SaveChangesAsync();

            return new();

        }
    }
}
