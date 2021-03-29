using System.Collections.Generic;
using System.Threading.Tasks;
using LetsDoIt.CustomValueTypes;
using LetsDoIt.CustomValueTypes.Email;
using LetsDoIt.CustomValueTypes.Image;

namespace LetsDoIt.Moody.Application.User
{
    using Persistence.Entities;


    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserAsync(int id);

        Task UpdateUserAsync(UserUpdateEntity userUpdateEntity);

        Task DeleteUserAsync(int modifiedById, int id);

        Task SaveUserAsync(Email email, string password, string fullName);

        Task SendActivationEmailAsync(string email);

        Task ActivateUserAsync(int id);
        
        Task<(int id, string token, string fullName)> AuthenticateAsync(Email email, string password);

        Task ForgetPasswordAsync(Email email);

        Task ResetPasswordAsync(int userId, string password);

        Task UpdateAccountDetails(int userId, string fullname, Email email, Image image = default);
    }
}
