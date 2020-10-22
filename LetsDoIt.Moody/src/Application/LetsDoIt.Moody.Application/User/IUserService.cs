using System.Threading.Tasks;
using LetsDoIt.Moody.Domain;

namespace LetsDoIt.Moody.Application.User
{
    public interface IUserService
    {
        Task SaveUserAsync(string userName,string password, string name = null, string surname = null, string email = null, UserTypes userType = UserTypes.Mobile);

        Task<UserTokenEntity> AuthenticateAsync(string username, string password);
        
        Task<bool> ValidateTokenAsync(string token);
    }
}
