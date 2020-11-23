using LetsDoIt.Moody.Persistence.Repositories.Base;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.User
{
    using LetsDoIt.Moody.Persistence.Entities;
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IRepository<User> _userRepository;

        public UserService(ILogger<UserService> logger, IRepository<User> userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<User> GetUser(int id)
        {
            var result = await _userRepository.SingleOrDefaultAsync(u => u.Id == id);

            return new User
            {
                Id = result.Id,
                FullName = result.FullName,
                Email = result.Email,
                IsActive = result.IsActive,
                UserType = result.UserType
            };
        }
    }
}
