using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NGuard;
using LetsDoIt.MailSender;
using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Application.User
{
    using Constants;
    using CustomExceptions;
    using Infrastructure.Utils;
    using Options;
    using Persistence.Entities;
    using Persistence.Repositories.Base;
    using Security;

    public class UserService : IUserService
    {
        private const string UserVerificationHtmlFilePath = @"HtmlTemplates/UserVerification.html";
        private const string EmailVerificationSubject = "Email Verification";
        private const string ActivateUserApiQuery = "activate-user";

        private const string ResetPasswordHtmlFilePath = @"HtmlTemplates/ResetPassword.html";
        private const string ResetPasswordSubject = "Reset Password";
        private const string ResetPasswordApiQuery = "reset-password";

        private readonly IRepository<User> _userRepository;
        private readonly IMailSender _mailSender;
        private readonly ISecurityService _securityService;
        private readonly string _activateUserApiUrl;
        private readonly string _resetPasswordApiUrl;

        public UserService(IRepository<User> userRepository,
            IMailSender mailSender,
            ISecurityService securityService,
            IOptions<WebInfoOptions> webInfoOptions)
        {
            _userRepository = userRepository;
            _mailSender = mailSender;
            _securityService = securityService;
            _activateUserApiUrl = $"{webInfoOptions.Value.Url}{ActivateUserApiQuery}";
            _resetPasswordApiUrl = $"{webInfoOptions.Value.Url}{ResetPasswordApiQuery}";
        }

        #region UserCRUD

        public async Task<IEnumerable<User>> GetUsersAsync()
            => await _userRepository.Get().Where(u => !u.IsDeleted).OrderBy(u => u.FullName).ToArrayAsync();

        public async Task<User> GetUserAsync(int id)
        {
            var dbUser = await _userRepository.GetAsync(u => u.Id == id && !u.IsDeleted);

            if (dbUser == null)
            {
                throw new UserNotFoundException();
            }

            return dbUser;
        }

        public async Task UpdateUserAsync(UserUpdateEntity userUpdateEntity)
        {
            var dbUser = await _userRepository.GetAsync(u => u.Id == userUpdateEntity.Id && !u.IsDeleted);

            if (dbUser == null)
            {
                throw new UserNotFoundException();
            }

            dbUser.Email = userUpdateEntity.Email;
            dbUser.FullName = userUpdateEntity.FullName;
            dbUser.ModifiedBy = userUpdateEntity.ModifiedById;
            dbUser.IsActive = userUpdateEntity.IsActive;
            dbUser.CanLogin = userUpdateEntity.CanLogin;

            CheckUserType(userUpdateEntity.UserType);

            dbUser.UserType = userUpdateEntity.UserType;

            if (userUpdateEntity.Password != null)
            {
                dbUser.Password = GetEncryptedPassword(userUpdateEntity.Email, userUpdateEntity.Password);
            }

            await _userRepository.UpdateAsync(dbUser);
        }

        public async Task DeleteUserAsync(int modifiedById, int id)
        {
            var dbUser = await _userRepository.GetAsync(u => u.Id == id && !u.IsDeleted);

            if (dbUser == null)
            {
                throw new UserNotFoundException();
            }

            dbUser.ModifiedBy = modifiedById;

            await _userRepository.DeleteAsync(dbUser);
        }

        public async Task SaveUserAsync(
            string email,
            string password,
            string fullName)
        {
            var isUserExisted = await _userRepository.AnyAsync(u => u.Email == email && !u.IsDeleted);

            if (isUserExisted)
            {
                throw new DuplicateNameException($"The email already exists in the system.");
            }

            await _userRepository.AddAsync(ToUser(email, password, fullName));

            await SendActivationEmailAsync(email);
        }

        #endregion

        public async Task SendActivationEmailAsync(string email)
        {
            var dbUser = await _userRepository.GetAsync(u => u.Email == email && !u.IsDeleted);

            if (dbUser == null)
            {
                throw new UserNotFoundException();
            }

            if (dbUser.IsActive)
            {
                throw new UserAlreadyActivatedException("Don't need to send activation email!");
            }

            var token = _securityService.GenerateJwtToken(dbUser.Id.ToString(), dbUser.FullName, UserTypeConstants.NotActivatedUser);

            var content = await ReadHtmlContentAsync(UserVerificationHtmlFilePath, _activateUserApiUrl, token.Token);

            await _mailSender.SendAsync(EmailVerificationSubject, content, email);
        }

        public async Task ActivateUserAsync(int id)
        {
            var dbUser = await _userRepository.GetAsync(u => u.Id == id && !u.IsDeleted);

            if (dbUser == null)
            {
                throw new UserNotFoundException();
            }

            if (dbUser.IsActive)
            {
                throw new UserAlreadyActivatedException("Don't need to activate it again!");
            }

            dbUser.IsActive = true;
            dbUser.ModifiedBy = dbUser.Id;

            await _userRepository.UpdateAsync(dbUser);
        }

        public async Task<(int id, string token, string fullName)> AuthenticateAsync(string email, string password)
        {
            Guard.Requires(email, nameof(email)).IsNotNullOrEmptyOrWhiteSpace();
            Guard.Requires(password, nameof(password)).IsNotNullOrEmptyOrWhiteSpace();

            var encryptedPassword = GetEncryptedPassword(email, password);

            var user = await _userRepository.GetAsync(u => u.Email == email && u.Password == encryptedPassword);

            ValidateUser(user);

            var tokenInfo = _securityService.GenerateJwtToken(user.Id.ToString(), user.FullName, user.UserType);

            if (tokenInfo == null)
            {
                throw new Exception($"Token can not be generated!" +
                                    $"UserId={user.Id};" +
                                    $"{nameof(user.FullName)}={user.FullName};" +
                                    $"{nameof(user.UserType)}={user.UserType}");
            }

            return (user.Id, tokenInfo.Token, user.FullName);
        }

        public async Task ForgetPasswordAsync(string email)
        {
            Guard.Requires(email, nameof(email)).IsNotNullOrEmptyOrWhiteSpace();

            var user = await _userRepository.GetAsync(u => u.Email == email && !u.IsDeleted);

            ValidateUser(user);

            var token = _securityService.GenerateJwtToken(user.Id.ToString(), user.FullName, UserTypeConstants.ResetPassword);

            var content = await ReadHtmlContentAsync(ResetPasswordHtmlFilePath, _resetPasswordApiUrl, token.Token);

            await _mailSender.SendAsync(ResetPasswordSubject, content, email.ToString());
        }

        public async Task ResetPasswordAsync(int userId, string password)
        {
            Guard.Requires(userId, nameof(userId)).IsGreaterThan(0);
            Guard.Requires(password, nameof(password)).IsNotNullOrEmptyOrWhiteSpace();

            var user = await _userRepository.GetAsync(u => u.Id == userId && !u.IsDeleted);

            ValidateUser(user);

            user.Password = GetEncryptedPassword(user.Email, password);
            user.ModifiedBy = userId;

            await _userRepository.UpdateAsync(user);
        }

        public async Task UpdateAccountDetails(int userId, string fullname, string email, string image = null)
        {
            var dbUser = await _userRepository.GetAsync(u => u.Id == userId && !u.IsDeleted);
            if (dbUser == null)
            {
                throw new UserNotFoundException();
            }

            dbUser.FullName = fullname;
            dbUser.Image = image == null ? dbUser.Image : Convert.FromBase64String(image);
            dbUser.Email = email;
            dbUser.ModifiedBy = userId;

            await _userRepository.UpdateAsync(dbUser);
        }

        private static void ValidateUser(User user)
        {
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            if (!user.IsActive)
            {
                throw new UserNotActiveException();
            }

            if (!user.CanLogin)
            {
                throw new UserNotHaveLoginPermissionException();
            }
        }

        private static async Task<string> ReadHtmlContentAsync(string filePath, string url, string token)
        {
            await using FileStream fileStream = new FileStream(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, filePath)), FileMode.Open);

            using StreamReader streamReader = new StreamReader(fileStream, Encoding.Unicode);

            var content = await streamReader.ReadToEndAsync();

            content = content.Replace("{{action_url}}", $"{url}?token={token}");

            return content;
        }

        private User ToUser(string email, string password, string fullName) => new User
        {
            Email = email,
            Password = GetEncryptedPassword(email, password),
            FullName = fullName
        };

        private string GetEncryptedPassword(string email, string password) => ProtectionHelper.EncryptValue(email + password);

        private static void CheckUserType(string userType)
        {
            var contains = typeof(UserTypeConstants).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(prop => prop.GetRawConstantValue()?.ToString()).Contains(userType);

            if (!contains)
            {
                throw new MissingUserTypeException();
            }
        }
    }
}
