using LetsDoIt.MailSender;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using Microsoft.Extensions.Logging;
using NGuard;

namespace LetsDoIt.Moody.Application.User
{
    using Persistence.Entities;
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using LetsDoIt.Moody.Persistence.Repositories.Base;
    using LetsDoIt.Moody.Application.Security;
    using LetsDoIt.Moody.Application.CustomExceptions;
    using LetsDoIt.Moody.Application.Constants;

    public class UserService : IUserService
    {
        private const string EmailVerification = "Email Verification";
        private readonly IRepository<User> _userRepository;
        private readonly IMailSender _mailSender;
        private readonly string _applicationKey;
        private readonly int _tokenExpirationMinutes;
        private readonly int _emailVerificationTokenExpirationMinutes;
        private const string HtmlFilePath = @"\HtmlTemplates\EmailTokenVerification.html";
        private readonly ILogger<UserService> _logger;
        private readonly ISecurityService _securityService;


        public UserService(
            IRepository<User> userRepository,
            IMailSender mailSender,
            ILogger<UserService> logger,
            ISecurityService securityService)
           
        {
            _userRepository = userRepository;
            _mailSender = mailSender;
            _securityService = securityService;
            
           
        }

        public async Task<ICollection<SystemUsersGetResult>> GetSystemUsers()
        {
            var result = await _userRepository.GetListAsync();

            if (result == null)
            {
                throw new ArgumentNullException("result is a null argument!");
            }

            //aslinda bu atama isi controller'da olsa daha iyi olurdu.
            // burada sadece logic olur sonra burayi cagiran controller'sa kendine gore bir entty olusturup atama yapar, baska bir class'sa ona gore davranir di. ne dersiniz tasiyalim mi
            // controller'a? evet. O zaman size kolay gelsin :))) :) saolun. onemli degil gorusmek uzere
            return result.Select(ToUser).ToList();
        }

        public SystemUsersGetResult ToUser(User result) => new SystemUsersGetResult
        {
            Id = result.Id,
            Username = result.Username,
            FullName = result.FullName,
            Email = result.Email,
            IsActive = result.IsActive,
            UserType = result.UserType
        };

        public async Task<ICollection<SystemUserDetailsGetResult>> GetSystemUserDetails(int id)
        {
            var result = await _userRepository.GetListAsync();

            if (result == null)
            {
                throw new ArgumentNullException("result is a null argument!");
            }

            result = (List<User>)result.Where(result => result.Id.Equals(id)).ToList();

            return result.Select(ToUserDetails).ToList();
        }


        public SystemUserDetailsGetResult ToUserDetails(User result) => new SystemUserDetailsGetResult
        {
            Id = result.Id,
            Username = result.Username,
            FullName = result.FullName,
            Email = result.Email,
            IsActive = result.IsActive,
            UserType = result.UserType,
            CreatedDate = result.CreatedDate
        };

        public async Task SaveUserAsync(string Username, string Password, string Fullname, string Email, bool IsActive, string UserType, int CreatedBy)
        {
            Guard.Requires(Username, nameof(Username)).IsNotNullOrEmptyOrWhiteSpace();
            Guard.Requires(Password, nameof(Password)).IsNotNullOrEmptyOrWhiteSpace();

            _logger.LogInformation($"{nameof(SaveUserAsync)} executing with username={Username}...");

            var isUserExisted = await _userRepository.AnyAsync(u => u.Username == Username);
            if (isUserExisted)
            {
                throw new DuplicateNameException($"The username of the user is already in the database. Username = {Username}");
            }

            var user = ToUserEntity(Username, Password, Fullname, Email, IsActive, UserType, CreatedBy);

            await _userRepository.AddAsync(new User
            {
                Username = user.Username,
                Password = user.Password,
                FullName = user.Fullname,
                Email = user.Email,
                IsActive = user.IsActive,
                UserType = user.UserType,
                CreatedBy = user.CreatedBy
            });

            _logger.LogInformation($"{nameof(SaveUserAsync)} executed with username={Username}.");
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

        private static async Task<string> ReadHtmlContent(string filePath, string referer, string token)
        {
            await using FileStream fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath), FileMode.Open);

            using StreamReader streamReader = new StreamReader(fileStream, Encoding.Unicode);

            var content = await streamReader.ReadToEndAsync();

            var frontEndUri = new Uri(referer);

            content = content.Replace("{{action_url}}",
                frontEndUri.Scheme + "://" + frontEndUri.Host
                          + "?token=" + token);

            return content;
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

        private static UserEntity ToUserEntity(string username, string password, string fullname, string email, bool isActive, string userType, int createdBy) =>
           new UserEntity
           (
               username,
               password,
               fullname,
               email,
               isActive,
               userType,
               createdBy
           );
    }
}