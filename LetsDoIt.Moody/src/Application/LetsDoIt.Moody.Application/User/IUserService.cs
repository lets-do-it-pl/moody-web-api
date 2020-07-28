using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.User
{
    public interface IUserService
    {
        Task SaveUserAsync(string userName,string password);
    }
}
