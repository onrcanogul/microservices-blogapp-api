using User.Service.Models.Dtos;

namespace User.Service.Features.Queries.GetUserById
{
    public class GetUserByIdQueryResponse
    {
        public UserDto User { get; set; }
    }
}