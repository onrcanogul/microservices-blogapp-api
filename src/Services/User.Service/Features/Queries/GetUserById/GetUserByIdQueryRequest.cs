using MediatR;

namespace User.Service.Features.Queries.GetUserById
{
    public class GetUserByIdQueryRequest : IRequest<GetUserByIdQueryResponse>
    {
        public string UserId { get; set; }
    }
}