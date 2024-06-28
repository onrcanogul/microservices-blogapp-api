using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Post.Service.Exceptions;
using Post.Service.Models.Contexts;

namespace Post.Service.Features.Commands.DeletePost
{
    public class DeletePostCommandHandler(PostDbContext context) : IRequestHandler<DeletePostCommandRequest, DeletePostCommandResponse>
    {
        public async Task<DeletePostCommandResponse> Handle(DeletePostCommandRequest request, CancellationToken cancellationToken)
        {
            Models.Entities.Post? post = await context.Posts.FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.PostId));
            EntityEntry entityEntry = null;
            if (post is not null)
            {
                entityEntry = context.Posts.Remove(post);
                await context.SaveChangesAsync();
            }
            else
                throw new PostNotFoundException(request.PostId);

            return new()
            {
                IsSuccess = entityEntry.State == EntityState.Deleted
            };
        }
    }
}
