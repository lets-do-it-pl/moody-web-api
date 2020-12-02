using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.User
{
    using LetsDoIt.Moody.Persistence.Entities;
    public interface IUserService
    {
        Task<User> GetUserAsync(int id);
    }
}
