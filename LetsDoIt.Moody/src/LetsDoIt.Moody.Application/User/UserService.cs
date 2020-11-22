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
   

    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMailSender _mailSender;
        private readonly string _applicationKey;
        private readonly int _tokenExpirationMinutes;
        private readonly int _emailVerificationTokenExpirationMinutes;
        private const string HtmlFilePath = @"\HtmlTemplates\EmailTokenVerification.html";
        private readonly ILogger<UserService> _logger;

        public UserService(
            IRepository<User> userRepository,
            string applicationKey,
            int tokenExpirationMinutes,
            int _emailVerificationTokenExpirationMinutes, ILogger<UserService> logger)
           
        {
            _userRepository = userRepository;
            
            _applicationKey = applicationKey;
            _tokenExpirationMinutes = tokenExpirationMinutes;
           
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

        private static async Task<string> ReadHtmlContent()
        {
            await using (FileStream fileStream =
                new FileStream(AppDomain.CurrentDomain.BaseDirectory
                               + HtmlFilePath,
                    FileMode.Open))
            {
                using StreamReader streamReader = new StreamReader(fileStream, Encoding.Unicode);
                return await streamReader.ReadToEndAsync();
            }
        }

        public Task ActiveUserAsync(int userId)
        {
            throw new NotImplementedException();
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