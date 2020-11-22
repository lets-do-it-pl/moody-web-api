using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.User
{
    public interface IUserService
    {

        Task ActiveUserAsync(int userId);
        Task<ICollection<SystemUsersGetResult>> GetSystemUsers();
        Task<ICollection<SystemUserDetailsGetResult>> GetSystemUserDetails(int id);

        Task SaveUserAsync(string Username, string Password, string Fullname, string Email, bool IsActive, string UserType, int CreatedBy);



    }
}
