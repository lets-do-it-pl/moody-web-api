using LetsDoIt.Moody.Infrastructure.ValueTypes;
using System.Threading.Tasks;

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

        Task<User> GetUser(int id);
    }
}
