using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.User
{
    using Persistence.Entities;
    public interface IUserService
    {
        Task SaveUserAsync(string username, string password, string email, string name, string surname);

        Task SendActivationEmailAsync(string referer, string email);

        Task ActivateUser(int id);

        Task<User> GetUserAsync(int id);

        Task ChangePasswordAsync(int id, string newPassword);
    }
}
