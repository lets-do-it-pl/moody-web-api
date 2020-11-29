using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.User
{
    public interface IUserService
    {

       
        Task<ICollection<SystemUsersGetResult>> GetSystemUsers();
        Task<ICollection<SystemUserDetailsGetResult>> GetSystemUserDetails(int id);
        Task SendActivationEmailAsync(string referer, string email);
        Task SaveUserAsync(string Username, string Password, string Fullname, string Email, bool IsActive, string UserType, int CreatedBy);

        Task ActivateUser(int id);

    }
}
