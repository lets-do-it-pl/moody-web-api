using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.User
{
    using Utils;
    using Persistance.Repositories.Base;
    using Domain;
    using NGuard;
    using System.Linq;
    using System.Data;

    public class UserService : IUserService
    {
        private readonly IEntityRepository<User> _userRepository;

        public UserService(IEntityRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task SaveUserAsync(string username, string password)
        {
            Guard.Requires(username, nameof(username));
            Guard.Requires(password, nameof(password));

            var isUserExisted = _userRepository.Get().Where(u => u.UserName == username).Any();
            if (isUserExisted)
            {
                throw new DuplicateNameException($"The username is already in the database. Username = {username}");
            }

            await _userRepository.AddAsync(new User
            {
                UserName = username,
                Password = ProtectionHelper.EncryptValue(username + password)
            });
        }
    }
}
