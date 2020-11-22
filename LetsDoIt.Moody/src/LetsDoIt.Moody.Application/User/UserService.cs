using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LetsDoIt.Moody.Application.User
{
    using Persistence.Entities;
    using Persistence.Repositories.Base;


    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IRepository<User> _userRepository;

        public UserService(ILogger<UserService> logger, IRepository<User> userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task SaveUserAsync(string username, string password, string email, string name, string surname)
        {
            var userEntity = new UserEntity(username,password,name,surname,email);

            await _userRepository.AddAsync(new User
            {
                Username = userEntity.Username,
                Password = userEntity.Password,
                Email = userEntity.Email,
                FullName = userEntity.FullName
            });
        }

        public Task SendActivationEmailAsync(string referer, string email)
        {
            throw new System.NotImplementedException();
        }

        public Task ActivateUser(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
