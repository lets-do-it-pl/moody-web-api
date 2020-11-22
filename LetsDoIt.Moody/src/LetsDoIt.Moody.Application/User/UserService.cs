using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LetsDoIt.MailSender;
using LetsDoIt.Moody.Application.Constants;
using LetsDoIt.Moody.Application.CustomExceptions;
using LetsDoIt.Moody.Application.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LetsDoIt.Moody.Application.User
{
    using Persistence.Entities;
    using Persistence.Repositories.Base;


    public class UserService : IUserService
    {
        private const string HtmlFilePath = @"\HtmlTemplates\EmailTokenVerification.html";
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
            var isUserExisted = await _userRepository.Get().AnyAsync(u => u.Username == username && !u.IsDeleted);

            if (isUserExisted)
            {
                throw new DuplicateNameException($"The username is already in the database. Username = {username}");
            }

            var userEntity = new UserEntity(username, password, name, surname, email);

            await _userRepository.AddAsync(new User
            {
                Username = userEntity.Username,
                Password = userEntity.Password,
                Email = userEntity.Email,
                FullName = userEntity.FullName
            });
        }

        public async Task SendActivationEmailAsync(string referer, string email)
        {
            var dbUser = await _userRepository.GetAsync(u => u.Email == email && !u.IsDeleted);

            if (dbUser == null)
            {
                throw new EmailNotRegisteredException(email);
            }

            var content = await ReadHtmlContent(HtmlFilePath);

            var frontEndUri = new Uri(referer);

            var token = _securityService.GenerateJwtToken(dbUser.Id.ToString(), dbUser.FullName, UserTypeConstants.Standard);

            await _mailSender.SendAsync(EmailVerification,
                content.Replace("{{action_url}}",
                    "http://" + frontEndUri.Host
                              + "?token=" + token.Token),
                email);
        }

        public async Task ActivateUser(int id)
        {
            var dbUser = await _userRepository.GetAsync(u => u.Id == id && !u.IsDeleted);

            if (dbUser == null)
            {
                throw new UserNotFoundException(id);
            }

            await _userRepository.UpdateAsync(new User
            {
                IsActive = true
            });
        }

        private static async Task<string> ReadHtmlContent(string filePath)
        {
            await using FileStream fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + filePath, FileMode.Open);

            using StreamReader streamReader = new StreamReader(fileStream, Encoding.Unicode);

            return await streamReader.ReadToEndAsync();
        }
    }
}
