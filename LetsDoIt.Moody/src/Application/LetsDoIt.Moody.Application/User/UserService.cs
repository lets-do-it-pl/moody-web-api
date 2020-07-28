using System;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.Utils;
using LetsDoIt.Moody.Persistance.Repositories.Base;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace LetsDoIt.Moody.Application.User
{
    public class UserService:IUserService
    {
        private readonly IEntityRepository<Domain.User> _userRepository;
        private readonly ProtectionHelper _protectionHelper;

        public UserService(IEntityRepository<Domain.User> userRepository, ProtectionHelper protectionHelper)
        {
            _userRepository = userRepository;
            _protectionHelper = protectionHelper;
        }

        public async Task SaveUserAsync(string userName, string password)
        {
          await  _userRepository.AddAsync(new Domain.User
            {
                UserName = userName,
                Password = _protectionHelper.EncryptValue(userName+password)

            }); 
        }

     

    }
}
