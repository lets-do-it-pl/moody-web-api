using System;
using System.Threading.Tasks;
using LetsDoIt.Moody.Persistance.Repositories.Base;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace LetsDoIt.Moody.Application.User
{
    public class UserService:IUserService
    {
        private readonly IEntityRepository<Domain.User> _userRepository;
        public UserService(IEntityRepository<Domain.User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task SaveUserAsync(string userName, string password)
        {
          await  _userRepository.AddAsync(new Domain.User
            {
                UserName = userName,
                Password = EncryptUserNameAndPassword(userName,password)

            });
        }

        public string EncryptUserNameAndPassword(string username, string password)
        {
            string user = username + password;
            byte[] salt = new byte[128 / 8];

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }

    }
}
