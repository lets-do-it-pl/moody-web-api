using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.User
{
    public interface IUserService
    {
        Task SaveUserAsync(string username, string password, string email, string name, string surname);

        Task SendActivationEmailAsync(string referer, string email);

        Task ActivateUserAsync(int id);
    }
}
