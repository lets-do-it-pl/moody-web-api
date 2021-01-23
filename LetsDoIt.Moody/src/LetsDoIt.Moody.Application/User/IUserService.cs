using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.User
{
    using Persistence.Entities;

    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task UpdateUserAsync(int modifiedById, int id, string username, string email, string name, string surname, string password = null);

        Task DeleteUserAsync(int modifiedById, int id);

        Task SaveUserAsync(string username, string password, string email, string name, string surname);

        Task SendActivationEmailAsync(string email);

        Task ActivateUserAsync(int id);
        
        Task<(int id, string token)> AuthenticateAsync(string email, string password);

        Task ForgetPasswordAsync(string email);

        Task ResetPasswordAsync(int userId, string password);
    }
}
