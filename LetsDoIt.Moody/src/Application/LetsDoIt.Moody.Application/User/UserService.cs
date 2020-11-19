using LetsDoIt.Moody.Infrastructure.ValueTypes;
using NGuard;
using System.Data;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.User
{
    using Domain;
    using Infrastructure.Utils;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Persistance.Repositories.Base;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Authentication;
    using System.Security.Claims;
    using System.Text;

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

        public async Task<ICollection<ToSystemUsersGetResult>> GetSystemUsers()
        {
            
            var result = await _userRepository.GetListAsync();

            if (result == null)
            {
                throw new ArgumentNullException("There is no user!");
            }

            return result.Select(ToUser).ToList();
        }

        public async Task<ICollection<ToSystemUsersGetResult>> GetUser(int id)
        {
            var result = await _userRepository.GetListAsync(u => u.Id == id);

            return result.Select(ToUser).ToList();
        }

        ToSystemUsersGetResult ToUser(User result) => new ToSystemUsersGetResult
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

        private static UserEntity ToUserEntity
            (string username, 
            string password, 
            bool isActive = false, 
            UserType userType = UserType.Mobile, 
            string name = null, 
            string surname = null, 
            Email email = new Email()) =>
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
