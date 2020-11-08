using System.Threading.Tasks;
using NGuard;
using System.Data;
using System.IO;
using LetsDoIt.MailSender;
using LetsDoIt.Moody.Application.CustomExceptions;
using LetsDoIt.Moody.Infrastructure.ValueTypes;

namespace LetsDoIt.Moody.Application.User
{
    using Infrastructure.Utils;
    using Persistance.Repositories.Base;
    using Domain;
    using System.Security.Authentication;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using System.Security.Claims;
    using System;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;

    public class UserService : IUserService
    {
        private readonly IEntityRepository<User> _userRepository;
        private readonly IEntityRepository<UserToken> _userTokenRepository;
        private readonly IEntityRepository<EmailVerificaitonToken> _emailVerificationTokenRepository;
        private readonly IMailSender _mailSender;
        private readonly string _applicationKey;
        private readonly int _tokenExpirationMinutes;
        private readonly int _emailVerificationTokenExpirationMinutes;
        private const string HtmlFilePath = @"\HtmlTemplates\EmailTokenVerification.html";

        public UserService(
            IEntityRepository<User> userRepository,
            IEntityRepository<UserToken> userTokenRepository,
            string applicationKey,
            int tokenExpirationMinutes,
            int _emailVerificationTokenExpirationMinutes,
            IMailSender mailSender, IEntityRepository<EmailVerificaitonToken> emailVerificationTokenRepository)
        {
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _applicationKey = applicationKey;
            _tokenExpirationMinutes = tokenExpirationMinutes;
            _mailSender = mailSender;
            _emailVerificationTokenRepository = emailVerificationTokenRepository;
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
            Name = result.Name,
            Surname = result.Surname,
            Email = result.Email,
            IsActive = result.IsActive,
            UserType = result.UserType
        };

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

        public async Task<UserTokenEntity> AuthenticateAsync(string username, string password)
        {
            Guard.Requires(username, nameof(username)).IsNotNullOrEmptyOrWhiteSpace();
            Guard.Requires(password, nameof(password)).IsNotNullOrEmptyOrWhiteSpace();

            var user = ToUserEntity(username, password);

            var userDb = await _userRepository
                                    .Get()
                                    .FirstOrDefaultAsync(u =>
                                        u.UserName == user.Username &&
                                        u.Password == user.EncryptedPassword &&
                                        !u.IsDeleted);

            if (userDb == null)
            {
                throw new AuthenticationException();
            }

            if (userDb.IsActive == false)
            {
                throw new UserNotActiveException(userDb.UserName);
            }

            UserToken userToken;

            if (userDb.UserToken == null || userDb.UserToken.ExpirationDate < DateTime.UtcNow || userDb.UserToken.Token == null)
            {
                var newUserToken = GetNewUserToken(user);
                newUserToken.UserId = userDb.Id;

                userToken = await _userTokenRepository.UpdateAsync(newUserToken);
            }
            else
            {
                userToken = userDb.UserToken;
            }

            return new UserTokenEntity
            {
                Username = username,
                Token = userToken.Token,
                ExpirationDate = userToken.ExpirationDate.Value
            };
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token) || token.Split(new char[] { ' ' }).Length != 2)
            {
                return false;
            }

            return await _userTokenRepository.Get().AnyAsync(ut => ut.Token == token.Split(new char[] { ' ' })[1] &&
                                                        ut.ExpirationDate > DateTime.UtcNow
                                                        && !ut.User.IsDeleted
                                                        && ut.User.IsActive);
        }

        public async Task SendEmailTokenAsync(string email)
        {
            var dbUser = await _userRepository.GetAsync(u => u.Email == email && !u.IsDeleted);

            if (dbUser == null)
            {
                throw new EmailNotRegisteredException(email);
            }

            var emailVerificationToken = GenerateEmailVerificationToken(dbUser);

            if (await _emailVerificationTokenRepository.Get().AnyAsync(evt => evt.UserId == dbUser.Id))
            {
                await _emailVerificationTokenRepository.UpdateAsync(emailVerificationToken);
            }
            else
            {
                await _emailVerificationTokenRepository.AddAsync(emailVerificationToken);
            }

            var content = await ReadHtmlContent();

            await _mailSender.SendAsync("Email Verification",
                content.Replace("{{action_url}}", "front-end/" + emailVerificationToken.Token),
                email);
        }

        private EmailVerificaitonToken GenerateEmailVerificationToken(User dbUser) => new EmailVerificaitonToken
        {
            Token = Guid.NewGuid().ToString(),
            ExpirationDate = DateTime.UtcNow.AddMinutes(_emailVerificationTokenExpirationMinutes),
            UserId = dbUser.Id
        };

        public async Task VerifyEmailTokenAsync(string token)
        {
            var emailVerificationToken = await _emailVerificationTokenRepository.GetAsync(evt => evt.Token == token
                                                                           && !evt.User.IsDeleted);

            if (emailVerificationToken == null)
            {
                throw new AuthenticationException($"No such token : {token}");
            }

            if (emailVerificationToken.ExpirationDate < DateTime.UtcNow)
            {
                throw new TokenExpiredException(emailVerificationToken.User.Email);
            }

            var userDb = await _userRepository.GetAsync(u => u.Id == emailVerificationToken.UserId && !u.IsDeleted);

            userDb.IsActive = true;

            await _userRepository.UpdateAsync(userDb);

            await _emailVerificationTokenRepository.DeleteAsync(emailVerificationToken);
        }

        private UserToken GetNewUserToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_applicationKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.EncryptedPassword)
                }),

                Expires = DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes),
                SigningCredentials =
                    new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey),
                        SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new UserToken
            {
                Token = tokenHandler.WriteToken(token),
                ExpirationDate = token.ValidTo
            };
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

        private static UserEntity ToUserEntity(string username, string password, bool isActive = false, UserType userType = UserType.Mobile, string name = null, string surname = null, Email email = new Email()) =>
            new UserEntity
            (
                username,
                ProtectionHelper.EncryptValue(username + password),
                isActive,
                userType,
                name,
                surname,
                email
            );
    }
}
