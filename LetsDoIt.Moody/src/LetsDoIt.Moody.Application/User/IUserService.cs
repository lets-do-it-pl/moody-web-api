using System.Collections.Generic;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.Constants;

namespace LetsDoIt.Moody.Application.User
{
    using Persistence.Entities;


    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserAsync(int id);

        Task UpdateUserAsync(UserUpdateEntity userUpdateEntity);

        Task DeleteUserAsync(int modifiedById, int id);

        Task SaveUserAsync(string email, string password, string fullName);

        Task SendActivationEmailAsync(string email);

        Task ActivateUserAsync(int id);
        
        Task<(int id, string token, string fullName)> AuthenticateAsync(string email, string password);

        Task ForgetPasswordAsync(string email);

        Task ResetPasswordAsync(int userId, string password);
    }
}
