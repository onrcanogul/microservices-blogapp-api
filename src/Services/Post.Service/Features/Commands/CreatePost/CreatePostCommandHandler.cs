using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Post.Service.Models.Contexts;

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
            await context.SaveChangesAsync();
            return new()
            {
                IsSuccess = entityEntry.State == EntityState.Added
            };

        }
    }
}
