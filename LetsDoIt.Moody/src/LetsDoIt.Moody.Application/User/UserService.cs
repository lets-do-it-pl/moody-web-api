using LetsDoIt.MailSender;
using LetsDoIt.Moody.Infrastructure.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.User
{
    using Constants;
    using CustomExceptions;
    using Persistence.Entities;
    using Persistence.Repositories.Base;
    using Security;

    public class UserService : IUserService
    {
        private const string HtmlFilePath = @"HtmlTemplates\UserVerification.html";
        private const string EmailVerification = "Email Verification";
        private readonly ILogger<UserService> _logger;
        private readonly IRepository<User> _userRepository;
        private readonly IMailSender _mailSender;
        private readonly ISecurityService _securityService;

        public UserService(ILogger<UserService> logger,
            IRepository<User> userRepository,
            IMailSender mailSender,
            ISecurityService securityService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _mailSender = mailSender;
            _securityService = securityService;
        }

        public async Task SaveUserAsync(string username, string password, string email, string name, string surname)
        {

            var isUserExisted = await _userRepository.AnyAsync(u => u.Username == username && !u.IsDeleted);

            if (isUserExisted)
            {
                throw new DuplicateNameException($"The username is already in the database. Username = {username}");
            }

            await _userRepository.AddAsync(ToUser(username, password, name, surname, email));
        }

        public async Task SendActivationEmailAsync(string referer, string email)
        {
            var dbUser = await _userRepository.GetAsync(u => u.Email == email && !u.IsDeleted);

            if (dbUser == null)
            {
                throw new EmailNotRegisteredException(email);
            }

            var token = _securityService.GenerateJwtToken(dbUser.Id.ToString(), dbUser.FullName, UserTypeConstants.Standard);

            var content = await ReadHtmlContent(HtmlFilePath, referer, token.Token);

            await _mailSender.SendAsync(EmailVerification, content, email);
        }

        public async Task ActivateUser(int id)
        {
            var dbUser = await _userRepository.GetAsync(u => u.Id == id && !u.IsDeleted);

            if (dbUser == null)
            {
                throw new UserNotFoundException(id);
            }

            dbUser.IsActive = true;
            dbUser.ModifiedBy = dbUser.Id;

            await _userRepository.UpdateAsync(dbUser);
        }

        private static async Task<string> ReadHtmlContent(string filePath, string referer, string token)
        {
            await using FileStream fileStream = new FileStream(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory , filePath)), FileMode.Open);

            using StreamReader streamReader = new StreamReader(fileStream, Encoding.Unicode);

            var content = await streamReader.ReadToEndAsync();

            var frontEndUri = new Uri(referer);

            content = content.Replace("{{action_url}}",
                frontEndUri.Scheme + "://" + frontEndUri.Host
                          + "?token=" + token);

            return content;
        }
        private User ToUser(string username, string password, string name, string surname, string email) => new User
        {
            Username = username,
            Password = ProtectionHelper.EncryptValue(username + password),
            FullName = name + surname,
            Email = email
        };
    }
}
