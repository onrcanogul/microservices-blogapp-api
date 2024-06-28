using Comment.Service.Models.Dtos;
using Comment.Service.Services.Abstractions;
using MediatR;

namespace Comment.Service.Features.Queries.GetCommentById
{
    public class GetCommentByIdQueryHandler(ICommentService service) : IRequestHandler<GetCommentByIdQueryRequest, GetCommentByIdQueryResponse>
    {
        public async Task<GetCommentByIdQueryResponse> Handle(GetCommentByIdQueryRequest request, CancellationToken cancellationToken)
        {
            CommentDto comment = await service.GetCommentByIdAsync(request.CommentId);
            return new()
            {
                Comment = comment
            };
        }
    }
}
