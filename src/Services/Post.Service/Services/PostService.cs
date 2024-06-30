using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Post.Service.Exceptions;
using Post.Service.Models.Contexts;
using Post.Service.Models.Dtos;
using Post.Service.Models.Entities;
using Post.Service.Services.Abstractions;
using Shared.Events;
using System.Text.Json;

namespace Post.Service.Services
{
    public class PostService(PostDbContext context) : IPostService
    {
        public async Task<bool> CreatePostAsync(string title, string description, string userId)
        {
            Models.Entities.Post post = new()
            {
                CommentsCount = 0,
                Description = description,
                Title = title,
                UserId = userId,
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

            return entityEntry.State == EntityState.Added;

        }
        public async Task<bool> RemovePostAsync(string postId)
        {
            Models.Entities.Post? post = await context.Posts.FirstOrDefaultAsync(x => x.Id == Guid.Parse(postId));
            EntityEntry entityEntry = null;
            if (post is not null)
            {
                entityEntry = context.Posts.Remove(post);

                Guid idempotentToken = Guid.NewGuid();
                PostDeletedEvent postDeletedEvent = new()
                {
                    IdempotentToken = idempotentToken,
                    PostId = post.Id,
                    UserId = post.UserId
                };
                PostOutbox postOutbox = new()
                {
                    IdempotentToken = idempotentToken,
                    OccuredOn = DateTime.UtcNow,
                    Payload = JsonSerializer.Serialize(postDeletedEvent),
                    Type = postDeletedEvent.GetType().Name,
                    ProcessedOn = null
                };
                await context.AddAsync(postOutbox);
                await context.SaveChangesAsync();
            }
            else
                throw new PostNotFoundException(postId);

            return entityEntry.State == EntityState.Deleted;
        }
        public async Task UpdatePostAsync(string postId, string title, string description)
        {
            Models.Entities.Post? post = await context.Posts.FirstOrDefaultAsync(x => x.Id == Guid.Parse(postId));
            if (post is not null)
            {
                post.Title = title;
                post.Description = description;
                await context.SaveChangesAsync();
            }
            else
                throw new PostNotFoundException(postId);
        }
        public async Task<PostDto> GetPostByIdAsync(string postId)
        {
            Models.Entities.Post? post = await context.Posts.FirstOrDefaultAsync(x => x.Id == Guid.Parse(postId));
            if (post is not null)
                return new()
                {
                    CommentsCount = post.CommentsCount,
                    Description = post.Description,
                    Title = post.Title,
                    Id = post.Id,
                    UserId = post.UserId,
                    CreatedDate = post.CreatedDate,
                    UpdatedDate = post.UpdatedDate
                };
            throw new PostNotFoundException(postId);
        }
        public async Task<List<PostDto>> GetPostByUserIdAsync(string userId)
        {
            List<Models.Entities.Post> posts = await context.Posts.Where(x => x.UserId == userId).ToListAsync();
            return posts.Select(p => new PostDto
               {
                   CommentsCount = p.CommentsCount,
                   Description = p.Description,
                   Id = p.Id,
                   Title = p.Title,
                   UserId = p.UserId,
                   CreatedDate = p.CreatedDate,
                   UpdatedDate = p.UpdatedDate
               }).ToList();
            
        }
        public async Task<List<PostDto>> GetPostsAsync()
        {
            List<Models.Entities.Post> posts = await context.Posts.ToListAsync();

            return posts.Select(x => new PostDto
            {
                Id = x.Id,
                CommentsCount = x.CommentsCount,
                Description = x.Description,
                Title = x.Title,
                UserId = x.UserId,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate
            }).ToList();
            
        }
  
    }
}
