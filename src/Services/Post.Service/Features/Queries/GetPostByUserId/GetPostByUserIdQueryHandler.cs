using MediatR;
using Microsoft.EntityFrameworkCore;
using Post.Service.Models.Contexts;

namespace Post.Service.Features.Queries.GetPostByUserId
{
    public class GetPostByUserIdQueryHandler(PostDbContext context) : IRequestHandler<GetPostByUserIdQueryRequest, GetPostByUserIdQueryResponse>
    {
        public async Task<GetPostByUserIdQueryResponse> Handle(GetPostByUserIdQueryRequest request, CancellationToken cancellationToken)
        {
            List<Models.Entities.Post> posts = await context.Posts.Where(x => x.UserId == request.UserId).ToListAsync();
            return new()
            {
                Posts = posts.Select(p => new Models.Dtos.PostDto
                {
                    CommentsCount = p.CommentsCount,
                    Description = p.Description,
                    Id = p.Id,
                    Title = p.Title,
                    UserId = p.UserId,
                    CreatedDate = p.CreatedDate,
                    UpdatedDate = p.UpdatedDate
                }).ToList()
            };
        }
    }
}
