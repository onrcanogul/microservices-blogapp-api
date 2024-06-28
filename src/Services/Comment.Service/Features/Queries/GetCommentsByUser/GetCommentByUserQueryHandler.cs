using Comment.Service.Models.Dtos;
using Comment.Service.Services.Abstractions;
using MediatR;

namespace Comment.Service.Features.Queries.GetCommentsByUser
{
    public class GetCommentsByUserQueryHandler(ICommentService service) : IRequestHandler<GetCommentsByUserQueryRequest, GetCommentsByUserQueryResponse>
    {
        public async Task<GetCommentsByUserQueryResponse> Handle(GetCommentsByUserQueryRequest request, CancellationToken cancellationToken)
        {
            List<CommentDto> comments = await service.GetCommentByUserAsync(request.UserId);
            return new()
            {
                Comments = comments
            };
        }
    }
}
