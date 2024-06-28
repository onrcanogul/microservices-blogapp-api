using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User.Service.Exceptions;
using User.Service.Models.Dtos;
using User.Service.Models.Entities;
using User.Service.Services.Abstractions;

namespace User.Service.Services
{
    public class UserService(UserManager<AppUser> userManager) : IUserService
    {
        public async Task<bool> CreateUserAsync(string username, string email, string password, string confirmPassword)
        {        
            if(IsConfirmed(password, confirmPassword))
            {
                AppUser user = new()
                {
                    Email = email,
                    UserName = username,
                    Id = Guid.NewGuid().ToString(),
                    PostCount = 0,
                    CommentCount = 0,
                };
                IdentityResult result = await userManager.CreateAsync(user, password);
                return result.Succeeded;
            }
            throw new PasswordsNotMatchedException();
        }

        public async Task<UserDto> GetUserById(string id)
        {
            AppUser? user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if(user is not null)
            {
                //todo comments and posts will add
                UserDto userDto = new()
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    PostCount = user.PostCount,
                    CommentCount = user.CommentCount
                };
                return userDto;
            }
            throw new UserNotFoundException(id);
        }

        public async Task<List<UserDto>> GetUsers()
        {
            List<AppUser> users = await userManager.Users.ToListAsync();
            List<UserDto> userDtos = users.Select(x => new UserDto()
            {
                Id = x.Id,
                CommentCount = x.CommentCount,
                Email = x.Email,
                PostCount = x.PostCount,
                UserName = x.UserName
            }).ToList();
            return userDtos;
        }

        private bool IsConfirmed(string password, string confirmPassword) => password == confirmPassword;
    }
}
