using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.User
{
    public interface IUserService
    {
        Task SaveUserAsync(string userName,string password);

        Task<UserTokenEntity> AuthenticateAsync(string username, string password);
        
        Task<bool> ValidateTokenAsync(string token);
    }
}
