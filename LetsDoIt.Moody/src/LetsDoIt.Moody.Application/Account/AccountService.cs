using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.CustomExceptions;
using LetsDoIt.Moody.Application.User;
using LetsDoIt.Moody.Persistence.Repositories;
using LetsDoIt.Moody.Persistence.Repositories.Base;

namespace LetsDoIt.Moody.Application.Account
{
    using Persistence.Entities;
    public class AccountService : IAccountService
    {
        private readonly IRepository<User> _userRepository;

        public AccountService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> GetAccount(int userId)
        {
            var dbUser = await _userRepository.GetAsync(u => u.Id == userId && !u.IsDeleted);

            if (dbUser == null)
            {
                throw new UserNotFoundException();
            }

            return dbUser;
        }

        public async Task UpdateAccountDetails(int userId, string fullname, string email, string image = null)
        {
            var dbUser = await _userRepository.GetAsync(u => u.Id == userId && !u.IsDeleted);
            if (dbUser == null)
            {
                throw new UserNotFoundException();
            }

            dbUser.FullName = fullname;
            dbUser.Image = image == null ? dbUser.Image : Convert.FromBase64String(image);
            dbUser.Email = email;

            await _userRepository.UpdateAsync(dbUser);
        }
    }
}
