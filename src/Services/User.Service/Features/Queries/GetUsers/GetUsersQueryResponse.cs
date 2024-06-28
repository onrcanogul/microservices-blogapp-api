using User.Service.Models.Dtos;

namespace User.Service.Features.Queries.GetUsers
{
    public class GetUsersQueryResponse
    {
        public List<UserDto> Users { get; set; }
    }
}