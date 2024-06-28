using MediatR;
using Microsoft.EntityFrameworkCore;
using Post.Service.Models.Contexts;

namespace Post.Service.Features.Queries.GetPosts
{
    public class GetPostsQueryHandler(PostDbContext context) : IRequestHandler<GetPostsQueryRequest, GetPostsQueryResponse>
    {
        public async Task<GetPostsQueryResponse> Handle(GetPostsQueryRequest request, CancellationToken cancellationToken)
        {

            //todo request to comments
            List<Models.Entities.Post> posts = await context.Posts.ToListAsync();

            return new()
            {
                Posts = posts.Select(x => new Models.Dtos.PostDto
                {
                    Id = x.Id,
                    CommentsCount = x.CommentsCount,
                    Description = x.Description,
                    Title = x.Title,
                    UserId = x.UserId,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                }).ToList()
            };
        }
    }
}
