﻿using Comment.Service.Exceptions;
using Comment.Service.Models.Contexts;
using Comment.Service.Models.Dtos;
using Comment.Service.Models.Entities;
using Comment.Service.Services.Abstractions;
using MongoDB.Driver;
using Shared.Base;
using Shared.Events;
using System.Text.Json;

namespace Comment.Service.Services
{
    public class CommentService(IMongoDbService mongoDbService, CommentOutboxDbContext context) : ICommentService
    {
        
        public async Task<bool> CreateCommentAsync(string message, string userId, string postId)
        {     
            try
            {
                Models.Entities.Comment comment = new()
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Message = message,
                    UserId = userId,
                    PostId = Guid.Parse(postId)
                };
                Task insertTask = commentCollection.InsertOneAsync(comment);
                Guid idempotentToken = Guid.NewGuid();
                CommentCreatedEvent commentCreatedEvent = new()
                {
                    CommentId = comment.Id,
                    PostId = comment.PostId,
                    UserId = comment.UserId,
                    IdempotentToken = idempotentToken
                };
                AddToCommentOutboxWithoutSaving(commentCreatedEvent, idempotentToken);
                Task saveTask = context.SaveChangesAsync();
                await Task.WhenAll(insertTask,saveTask);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteCommentAsync(string commentId)
        {
            try
            {
                Models.Entities.Comment comment = await (await commentCollection.FindAsync(x => x.Id == Guid.Parse(commentId))).FirstOrDefaultAsync();
                if(comment is not null)
                { 
                    Task deleteComment = commentCollection.FindOneAndDeleteAsync(com => com.Id == Guid.Parse(commentId));
                    Guid idempotentToken = Guid.NewGuid();
                    CommentDeletedEvent commentDeletedEvent = new()
                    {
                        CommentId = comment.Id,
                        PostId = comment.PostId,
                        UserId = comment.UserId,
                        IdempotentToken = idempotentToken
                    };
                    AddToCommentOutboxWithoutSaving(commentDeletedEvent, idempotentToken);
                    Task saveChanges = context.SaveChangesAsync();
                    await Task.WhenAll(saveChanges, deleteComment);

                    return true;
                }
                else
                    throw new CommentNotFoundException(commentId);

            }
            catch
            {
                return false;
            }
        }

        public async Task<CommentDto> GetCommentByIdAsync(string commentId)
        {
           Models.Entities.Comment comment =  await (await commentCollection.FindAsync(x => x.Id == Guid.Parse(commentId))).FirstOrDefaultAsync();
            if(comment is not null) { 
            CommentDto commentDto = new()
            {
                Id = comment.Id,
                CreatedDate = DateTime.Now,
                Message = comment.Message,
                UserId = comment.UserId,
                PostId = comment.PostId
            };
                return commentDto;
            }
            throw new CommentNotFoundException(commentId);
        }

        public async Task<List<CommentDto>> GetCommentByUserAsync(string userId)
        {
            List<Models.Entities.Comment> comments = await(await commentCollection.FindAsync(x => x.UserId == userId)).ToListAsync();
            List<CommentDto> commentDtos = comments.Select(c => new CommentDto
            {
                CreatedDate = c.CreatedDate,
                Id = c.Id,
                Message = c.Message,
                PostId = c.PostId,
                UserId = c.UserId
            }).ToList();
            return commentDtos;
        }

        public async Task<List<CommentDto>> GetCommentsByPostAsync(string postId)
        {
            List<Models.Entities.Comment> comments = await(await commentCollection.FindAsync(x => x.PostId == Guid.Parse(postId))).ToListAsync();
            List<CommentDto> commentDtos = comments.Select(c => new CommentDto
            {
                CreatedDate = c.CreatedDate,
                Id = c.Id,
                Message = c.Message,
                PostId = c.PostId,
                UserId = c.UserId
            }).ToList();
            return commentDtos;
        }

        private IMongoCollection<Models.Entities.Comment> commentCollection => mongoDbService.GetCollection<Models.Entities.Comment>("Comments");


        private async void AddToCommentOutboxWithoutSaving(IEvent @event, Guid idempotentToken)
        {
            CommentOutbox commentOutbox = new()
            {
                IdempotentToken = idempotentToken,
                OccuredOn = DateTime.UtcNow,
                ProcessedOn = null,
                Type = @event.GetType().Name,
                Payload = JsonSerializer.Serialize(@event)
            };
            await context.CommentOutboxes.AddAsync(commentOutbox);
        }
    }
}
