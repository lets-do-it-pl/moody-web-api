using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.User
{
    using Utils;
    using Persistance.Repositories.Base;
    using Domain;

    public class UserService:IUserService
    {
        private readonly IEntityRepository<User> _userRepository;

        public UserService(IEntityRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task SaveUserAsync(string userName, string password)
        {
          await  _userRepository.AddAsync(new User
            {
                UserName = userName,
                Password = ProtectionHelper.EncryptValue(userName+password)

            }); 
        }

     

    }
}
