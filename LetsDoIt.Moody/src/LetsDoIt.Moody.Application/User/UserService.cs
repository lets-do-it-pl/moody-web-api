using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LetsDoIt.MailSender;
using LetsDoIt.Moody.Infrastructure.Utils;

namespace LetsDoIt.Moody.Application.User
{
    using Constants;
    using CustomExceptions;
    using Security;
    using Persistence.Entities;
    using Persistence.Repositories.Base;
    using NGuard;
    using Microsoft.Extensions.Options;
    using LetsDoIt.Moody.Application.Options;

    public class UserService : IUserService
    {
        private const string HtmlFilePath = @"HtmlTemplates\UserVerification.html";
        private const string EmailVerification = "Email Verification";
        private const string ActivateUserApiQuery = "activate-user";

        private readonly IRepository<User> _userRepository;
        private readonly IMailSender _mailSender;
        private readonly ISecurityService _securityService;
        private readonly string _activateUserApiUrl;

        public UserService(IRepository<User> userRepository,
            IMailSender mailSender,
            ISecurityService securityService,
            IOptions<WebInfoOptions> webInfoOptions)
        {
            _userRepository = userRepository;
            _mailSender = mailSender;
            _securityService = securityService;
            _activateUserApiUrl = $"{webInfoOptions.Value.Url}{ActivateUserApiQuery}";
        }

        public async Task SaveUserAsync(
            string username,
            string password,
            string email,
            string name,
            string surname)
        {
            var isUserExisted = await _userRepository.AnyAsync(u => u.Email == email && !u.IsDeleted);

            if (isUserExisted)
            {
                throw new DuplicateNameException($"The email already exists in the system. Email = {email}");
            }

            await _userRepository.AddAsync(ToUser(username, password, name, surname, email));

            await SendActivationEmailAsync(email);
        }

        public async Task SendActivationEmailAsync(string email)
        {
            var dbUser = await _userRepository.GetAsync(u => u.Email == email && !u.IsDeleted);

            if (dbUser == null)
            {
                throw new UserNotRegisteredException(email);
            }

            if (dbUser.IsActive)
            {
                throw new UserAlreadyActivatedException("Don't need to send activation email!");
            }

            var token = _securityService.GenerateJwtToken(dbUser.Id.ToString(), dbUser.FullName, UserTypeConstants.NotActivatedUser);

            var content = await ReadHtmlContent(HtmlFilePath, _activateUserApiUrl, token.Token);

            await _mailSender.SendAsync(EmailVerification, content, email);
        }

        public async Task ActivateUserAsync(int id)
        {
            var dbUser = await _userRepository.GetAsync(u => u.Id == id && !u.IsDeleted);

            if (dbUser == null)
            {
                throw new UserNotFoundException(id);
            }

            if (dbUser.IsActive)
            {
                throw new UserAlreadyActivatedException("Don't need to activate it again!");
            }

            dbUser.IsActive = true;
            dbUser.ModifiedBy = dbUser.Id;            

            await _userRepository.UpdateAsync(dbUser);
        }

        public async Task<(int id, string token)> AuthenticateAsync(string username, string password)
        {
            Guard.Requires(username, nameof(username)).IsNotNullOrEmptyOrWhiteSpace();
            Guard.Requires(password, nameof(password)).IsNotNullOrEmptyOrWhiteSpace();

            var encryptedPassword = ProtectionHelper.EncryptValue(username + password);

            var user = await _userRepository.GetAsync(u => u.Username == username && u.Password == encryptedPassword);

            if (user == null)
            {
                throw new UserNotFoundException(default);
            }

            if (!user.IsActive)
            {
                throw new UserNotActiveException();
            }

            if (!user.CanLogin)
            {
                throw new UserNotHaveLoginPermissionException();
            }

            var tokenInfo = _securityService.GenerateJwtToken(user.Id.ToString(), user.FullName, user.UserType);
            if (tokenInfo == null)
            {
                throw new Exception($"Token can not be generated!" +
                                    $"UserId={user.Id};" +
                                    $"{nameof(user.FullName)}={user.FullName};" +
                                    $"{nameof(user.UserType)}={user.UserType}");
            }

            return (user.Id, tokenInfo.Token);
        }

        private static async Task<string> ReadHtmlContent(string filePath, string url, string token)
        {
            await using FileStream fileStream = new FileStream(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, filePath)), FileMode.Open);

            using StreamReader streamReader = new StreamReader(fileStream, Encoding.Unicode);

            var content = await streamReader.ReadToEndAsync();

            content = content.Replace("{{action_url}}", $"{url}?token={token}");

            return content;
        }

        private User ToUser(string username, string password, string name, string surname, string email) => new User
        {
            Username = username,
            Password = ProtectionHelper.EncryptValue(username + password),
            FullName = $"{name} {surname}",
            Email = email
        };

    }
}
