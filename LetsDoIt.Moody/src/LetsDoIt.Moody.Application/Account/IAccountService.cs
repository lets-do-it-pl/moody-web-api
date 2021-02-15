using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Account
{
    public interface IAccountService
    {
        Task<Persistence.Entities.User> GetAccount(int userId);

        Task UpdateAccount(int userId);

    }
}
