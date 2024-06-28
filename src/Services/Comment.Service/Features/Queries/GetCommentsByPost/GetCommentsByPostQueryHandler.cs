using Comment.Service.Models.Dtos;
using Comment.Service.Services.Abstractions;
using MediatR;

namespace Comment.Service.Features.Queries.GetCommentsByPost
{
    public class GetCommentsByPostQueryHandler(ICommentService service) : IRequestHandler<GetCommentsByPostQueryRequest, GetCommentsByPostQueryResponse>
    {
        public async Task<GetCommentsByPostQueryResponse> Handle(GetCommentsByPostQueryRequest request, CancellationToken cancellationToken)
        {
            List<CommentDto> comments = await service.GetCommentsByPostAsync(request.PostId);
            return new()
            {
                Comments = comments
            };
        }
    }
}
