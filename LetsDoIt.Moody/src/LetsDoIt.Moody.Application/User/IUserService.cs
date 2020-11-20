using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.User
{
    public interface IUserService
    {

        Task SaveUserAsync();

        Task ActiveUserAsync(int userId);
        Task<ICollection<SystemUsersGetResult>> GetSystemUsers();
        Task<ICollection<SystemUserDetailsGetResult>> GetSystemUserDetails(int id);
        
    }
}
