using System.Threading.Tasks;
using NGuard;
using System.Linq;
using System.Data;

namespace LetsDoIt.Moody.Application.User
{
    using Utils;
    using Persistance.Repositories.Base;
    using Domain;
    using System.Security.Authentication;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using System.Security.Claims;
    using System;
    using Microsoft.EntityFrameworkCore;

    public class UserService : IUserService
    {
        private readonly IEntityRepository<User> _userRepository;
        private readonly IEntityRepository<UserToken> _userTokenRepository;
        private readonly string _applicationKey;
        private readonly int _tokenExpirationMinutes;

        public UserService(
            IEntityRepository<User> userRepository,
            IEntityRepository<UserToken> userTokenRepository,
            string applicationKey,
            int tokenExpirationMinutes)
        {
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _applicationKey = applicationKey;
            _tokenExpirationMinutes = tokenExpirationMinutes;
        }

        public async Task SaveUserAsync(string username, string password)
        {
            Guard.Requires(username, nameof(username));
            Guard.Requires(password, nameof(password));

            var isUserExisted = _userRepository.Get().Where(u => u.UserName == username && !u.IsDeleted).Any();
            if (isUserExisted)
            {
                throw new DuplicateNameException($"The username is already in the database. Username = {username}");
            }

            var newUser = GetUser(username, password);

            await _userRepository.AddAsync(new User
            {
                UserName = newUser.Username,
                Password = newUser.EncryptedPassword
            });
        }

        public async Task<UserTokenEntity> AuthenticateAsync(string username, string password)
        {
            Guard.Requires(username, nameof(username));
            Guard.Requires(password, nameof(password));

            var user = GetUser(username, password);

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

            UserToken userToken;

            if(userDb.UserToken == null || userDb.UserToken.ExpirationDate < DateTime.Now)
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

        private UserToken GetNewUserToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_applicationKey);

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

        private static UserEntity GetUser(string username, string password) =>
            new UserEntity
            (
                username,
                ProtectionHelper.EncryptValue(username + password)
            );
    }
}
