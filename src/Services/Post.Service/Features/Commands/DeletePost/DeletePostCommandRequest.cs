using MediatR;

namespace Post.Service.Features.Commands.DeletePost
{
    public class DeletePostCommandRequest : IRequest<DeletePostCommandResponse>
    {
        public string PostId { get; set; } = null!;
    }
}