using System.Threading.Tasks;
using LetsDoIt.Moody.Domain;

namespace LetsDoIt.Moody.Application.User
{
    public interface IUserService
    {
        Task SaveUserAsync(string userName,string password, bool isActive = false, UserTypes userType = UserTypes.Mobile, string name = null, string surname = null, string email = null);

        Task<UserTokenEntity> AuthenticateAsync(string username, string password);
        
        Task<bool> ValidateTokenAsync(string token);

        Task<bool> SendEmailTokenAsync(string email);

        Task<bool> VerifyEmailTokenAsync(string token);

    }
}
