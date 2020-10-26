using System.Threading.Tasks;
using LetsDoIt.Moody.Domain;

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
            string email = null );

        Task<UserTokenEntity> AuthenticateAsync(string username, string password);
        
        Task<bool> ValidateTokenAsync(string token);

        Task SendEmailTokenAsync(string email);

        Task VerifyEmailTokenAsync(string token);

    }
}
