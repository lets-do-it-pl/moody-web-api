using System.Collections.Generic;
using System.Threading.Tasks;
using LetsDoIt.Moody.Domain;
using LetsDoIt.Moody.Infrastructure.ValueTypes;

namespace LetsDoIt.Moody.Application.User
{
    public interface IUserService
    {
        Task SaveUserAsync(
            string userName,
            string password,
            bool isActive = false,
            UserType userType = UserType.Mobile,
            string name = null,
            string surname = null, 
            Email email = new Email() );

        Task<UserTokenEntity> AuthenticateAsync(string username, string password);
        
        Task<bool> ValidateTokenAsync(string token);

        Task<ICollection<SystemUsersGetResult>> GetSystemUsers();

        Task<ICollection<SystemUsersGetResult>> GetUserById(int id);
    }
}
