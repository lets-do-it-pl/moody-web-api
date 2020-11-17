using Microsoft.Extensions.Logging;

namespace LetsDoIt.Moody.Application.User
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }
    }
}
