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
            Email email = new Email());

        Task<UserTokenEntity> AuthenticateAsync(string username, string password);

        Task<bool> ValidateTokenAsync(string token);

        Task SendForgotEmailAsync(string referer, string email);

        Task ForgotEmailTokenAsync(string token);

    }
}
