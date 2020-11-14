using LetsDoIt.MailSender;
using NGuard;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.User
{
    using CustomExceptions;
    using Domain;
    using Infrastructure.Utils;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Persistance.Repositories.Base;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Authentication;
    using System.Security.Claims;
    using System.Text;
    public class UserService : IUserService
    {
        private const string HtmlFilePath = @"\HtmlTemplates\EmailTokenVerification.html";
        private const string EmailVerification = "Email Verification";
        private readonly IEntityRepository<User> _userRepository;
        private readonly IEntityRepository<UserToken> _userTokenRepository;
        private readonly IEntityRepository<EmailVerificaitonToken> _emailVerificationTokenRepository;
        private readonly IMailSender _mailSender;
        private readonly string _applicationKey;
        private readonly int _tokenExpirationMinutes;
        private readonly int _emailVerificationTokenExpirationMinutes;

        public UserService(
            IEntityRepository<User> userRepository,
            IEntityRepository<UserToken> userTokenRepository,
            string applicationKey,
            int tokenExpirationMinutes,
            int emailVerificationTokenExpirationMinutes,
            IMailSender mailSender, IEntityRepository<EmailVerificaitonToken> emailVerificationTokenRepository)
        {
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _applicationKey = applicationKey;
            _tokenExpirationMinutes = tokenExpirationMinutes;
            _emailVerificationTokenExpirationMinutes = emailVerificationTokenExpirationMinutes;
            _mailSender = mailSender;
            _emailVerificationTokenRepository = emailVerificationTokenRepository;
        }

        public async Task SaveUserAsync(string username,
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

            await _userRepository.AddAsync(new User
            {
                UserName = username,
                Password = ProtectionHelper.EncryptValue(username + password),
                Name = name,
                Surname = surname,
                Email = email,
                UserType = userType,
                IsActive = isActive
            });
        }

        public async Task<UserTokenEntity> AuthenticateAsync(string username, string password)
        {
            Guard.Requires(username, nameof(username)).IsNotNullOrEmptyOrWhiteSpace();
            Guard.Requires(password, nameof(password)).IsNotNullOrEmptyOrWhiteSpace();

            var user = new User
            {
                UserName = username,
                Password = ProtectionHelper.EncryptValue(username + password)
            };

            var userDb = await _userRepository
                .Get()
                .FirstOrDefaultAsync(u =>
                    u.UserName == user.UserName &&
                    u.Password == user.Password &&
                    !u.IsDeleted);

            if (userDb == null)
            {
                throw new AuthenticationException();
            }

            if (!userDb.IsActive)
            {
                throw new UserNotActiveException(userDb.UserName);
            }

            UserToken userToken;

            if (userDb.UserToken == null || userDb.UserToken.ExpirationDate < DateTime.UtcNow ||
                userDb.UserToken.Token == null)
            {
                var newUserToken = GetNewUserToken(ProtectionHelper.EncryptValue(username + password));
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
            if (string.IsNullOrWhiteSpace(token) || token.Split(new char[] {' '}).Length != 2)
            {
                return false;
            }

            return await _userTokenRepository.Get().AnyAsync(ut => ut.Token == token.Split(new char[] {' '})[1] &&
                                                                   ut.ExpirationDate > DateTime.UtcNow
                                                                   && !ut.User.IsDeleted
                                                                   && ut.User.IsActive);
        }

        public async Task SendSignUpEmailAsync(string referer, string email)
        {
            var dbUser = await _userRepository.GetAsync(u => u.Email == email && !u.IsDeleted);

            if (dbUser == null)
            {
                throw new EmailNotRegisteredException(email);
            }

            var emailVerificationToken = GenerateEmailVerificationToken(dbUser.Id);

            if (await _emailVerificationTokenRepository.Get().AnyAsync(evt => evt.UserId == dbUser.Id))
            {
                await _emailVerificationTokenRepository.UpdateAsync(emailVerificationToken);
            }
            else
            {
                await _emailVerificationTokenRepository.AddAsync(emailVerificationToken);
            }

            var content = await ReadHtmlContent(HtmlFilePath);

            var frontEndUri = new Uri(referer);

            await _mailSender.SendAsync(EmailVerification,
                content.Replace("{{action_url}}", "http://" + frontEndUri.Host
                                                            + "?token=" + emailVerificationToken.Token),
                email);
        }

        private EmailVerificaitonToken GenerateEmailVerificationToken(int id) => new EmailVerificaitonToken
        {
            Token = Guid.NewGuid().ToString(),
            ExpirationDate = DateTime.UtcNow.AddMinutes(_emailVerificationTokenExpirationMinutes),
            UserId = id
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

        private UserToken GetNewUserToken(string encryptedPassword)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_applicationKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, encryptedPassword)
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

        private static async Task<string> ReadHtmlContent(string filePath)
        {
            await using FileStream fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + filePath, FileMode.Open);

            using StreamReader streamReader = new StreamReader(fileStream, Encoding.Unicode);

            return await streamReader.ReadToEndAsync();
        }
    }
}
