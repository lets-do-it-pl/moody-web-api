using Microsoft.Extensions.Logging;

namespace LetsDoIt.Moody.Application.User
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IEntityRepository<User> _userRepository;

        public UserService(ILogger<UserService> logger, IEntityRepository<User> userRepository)
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
                Name = result.Name,
                Surname = result.Surname,
                Email = result.Email,
                IsActive = result.IsActive,
                UserType = result.UserType
            };
        }

        public async Task SaveUserAsync(
            string username,
            string password,
            bool isActive = false,
            UserType userType = UserType.Mobile,
            string name = null,
            string surname = null,
            Email email = new Email())
        {
            Guard.Requires(username, nameof(username)).IsNotNullOrEmptyOrWhiteSpace();
            Guard.Requires(password, nameof(password)).IsNotNullOrEmptyOrWhiteSpace();

            var isUserExisted = await _userRepository.Get().AnyAsync(u => u.UserName == username && !u.IsDeleted);
            if (isUserExisted)
            {
                throw new DuplicateNameException($"The username is already in the database. Username = {username}");
            }

            var newUser = ToUserEntity(username, password, isActive, userType, name, surname, email);

            await _userRepository.AddAsync(new User
            {
                UserName = newUser.Username,
                Password = newUser.EncryptedPassword,
                Name = newUser.Name,
                Surname = newUser.Surname,
                Email = newUser.Email.ToString(),
                UserType = newUser.UserType,
                IsActive = newUser.IsActive
            });
        }
    }
}
