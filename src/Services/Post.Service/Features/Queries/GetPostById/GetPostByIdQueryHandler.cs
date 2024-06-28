using MediatR;
using Microsoft.EntityFrameworkCore;
using Post.Service.Exceptions;
using Post.Service.Models.Contexts;

namespace Post.Service.Features.Queries.GetPostById
{
    public class GetPostByIdQueryHandler(PostDbContext context) : IRequestHandler<GetPostByIdQueryRequest, GetPostByIdQueryResponse>
    {
        public async Task<GetPostByIdQueryResponse> Handle(GetPostByIdQueryRequest request, CancellationToken cancellationToken)
        {
            Models.Entities.Post? post = await context.Posts.FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.PostId));
            if (post is not null) 
            {
                return new()
                {
                    Post = new()
                    {
                        CommentsCount = post.CommentsCount,
                        Description = post.Description,
                        Title = post.Title,
                        Id = post.Id,
                        UserId = post.UserId,
                        CreatedDate = post.CreatedDate,
                        UpdatedDate = post.UpdatedDate
                    }
                };
            }
            throw new PostNotFoundException(request.PostId);
        }
    }
}
