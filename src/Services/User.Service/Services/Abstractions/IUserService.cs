using User.Service.Models.Dtos;

namespace User.Service.Services.Abstractions
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(string username, string email, string password, string confirmPassword);
        Task<List<UserDto>> GetUsers();
        Task<UserDto> GetUserById(string id);
    }
}
