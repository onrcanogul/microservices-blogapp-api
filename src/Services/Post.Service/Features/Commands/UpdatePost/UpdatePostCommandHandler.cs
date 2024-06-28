using MediatR;
using Microsoft.EntityFrameworkCore;
using Post.Service.Exceptions;
using Post.Service.Models.Contexts;

namespace Post.Service.Features.Commands.UpdatePost
{
    public class UpdatePostCommandHandler(PostDbContext context) : IRequestHandler<UpdatePostCommandRequest, UpdatePostCommandResponse>
    {
        public async Task<UpdatePostCommandResponse> Handle(UpdatePostCommandRequest request, CancellationToken cancellationToken)
        {
            Models.Entities.Post? post = await context.Posts.FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.PostId));
            if (post is not null)
            {
                post.Title = request.Title;
                post.Description = request.Description;
                await context.SaveChangesAsync();
            }
            else
                throw new PostNotFoundException(request.PostId);

            return new();
            
        }
    }
}
