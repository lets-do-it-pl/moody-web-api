using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.User
{
    public interface IUserService
    {
        
        Task SendSignUpEmailAsync(string referer, string email);

        Task SaveUserAsync();

        Task ActiveUserAsync(int userId);

    }
}
