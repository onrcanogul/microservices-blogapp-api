using MediatR;

namespace User.Service.Features.Queries.GetUsers
{
    public class GetUsersQueryRequest : IRequest<GetUsersQueryResponse>
    {
    }
}