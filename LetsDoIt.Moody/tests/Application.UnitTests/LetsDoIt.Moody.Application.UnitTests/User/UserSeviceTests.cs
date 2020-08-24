using Moq;
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Authentication;
using MockQueryable.Moq;

namespace LetsDoIt.Moody.Application.UnitTests
{
    using Domain;
    using Application.User;
    using Application.Utils;
    using Persistance.Repositories.Base;

    public class UserSeviceTests
    {
        private readonly UserService _testing;
        private readonly Mock<IEntityRepository<UserToken>> _mockUserTokenRepository;
        private readonly Mock<IEntityRepository<User>> _mockUserRepository;
        private readonly string _applicationKey = "d1442e0f-01e0-4074-bdae-28b8f57a6b40";
        private readonly int _tokenExpirationMinutes = 123;

        public UserSeviceTests()
        {
            _mockUserRepository = new Mock<IEntityRepository<User>>();
            _mockUserTokenRepository = new Mock<IEntityRepository<UserToken>>();
            _testing = new UserService
                (_mockUserRepository.Object,
                _mockUserTokenRepository.Object,
                _applicationKey,
                _tokenExpirationMinutes);
        }

        [Fact]
        public async Task AuthenticateAsync_UserExistsAndTokenIsNotNull_ReturnToken()
        {
            //Arrange
            var username = "good.username";
            var password = "good.password";

            var token = new UserToken
            {
                Token = "good.token",
                ExpirationDate = DateTime.Now.AddMinutes(12)
            };

            var users = new List<User>
            {
                new User()
                {
                    UserName = username,
                    Password = ProtectionHelper.EncryptValue(username + password),
                    UserToken = token
                }

            };

            //Act
            _mockUserRepository.Setup(repo => repo.Get()).
                Returns(users.AsQueryable().BuildMockDbSet().Object);

            var actual = await _testing.AuthenticateAsync(username, password);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(username, actual.Username);
            Assert.Equal(token.Token, actual.Token);
            Assert.True(DateTime.Now < actual.ExpirationDate);
        }

        [Fact]
        public async Task AuthenticateAsync_UserDoesNotExistsInTheDatabase_ThrowAuthenticationException()
        {
            var username = "bad.username";
            var password = "bad.password";

            async Task Test() => await _testing.AuthenticateAsync(username, password);

            await Assert.ThrowsAsync<AuthenticationException>(Test);
        }

        [Fact]
        public async Task AuthenticateAsync_UserExistsWithoutToken_ShouldGenerateAToken()
        {
            var username = "good.username";
            var password = "good.password";

            var users = new List<User>
            {
                new User()
                {
                    Id = 1,
                    UserName = username,
                    Password = ProtectionHelper.EncryptValue(username + password),
                    UserToken = null
                }

            };

            var userToken = new UserToken
            {
                UserId = 1,
                Token = "good.token",
                ExpirationDate = DateTime.Now.AddMinutes(5)
            };

            _mockUserTokenRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<UserToken>()))
                .ReturnsAsync(userToken);

            _mockUserRepository.Setup(user => user.Get()).
                Returns(users.AsQueryable().BuildMockDbSet().Object);

             var actual = await _testing.AuthenticateAsync(username, password);

            _mockUserTokenRepository.Verify(token =>
                    token.UpdateAsync(It.IsAny<UserToken>()),
                    Times.Once);
        }

        [Fact]
        public async Task AuthenticateAsync_UserExistsWithExpiredToken_ShouldGenerateNewToken()
        {
            var username = "good.username";
            var password = "good.password";
            
            var expiredUserToken = new UserToken
            {
                UserId = 1,
                Token = "expired.token",
                ExpirationDate = DateTime.Now
            };

            var userToken = new UserToken
            {
                UserId = 1,
                Token = "good.token",
                ExpirationDate = DateTime.Now.AddMinutes(5)
            };

            var users = new List<User>
            {
                new User()
                {
                    Id = 1,
                    UserName = username,
                    Password =  ProtectionHelper.EncryptValue(username + password),
                    UserToken = expiredUserToken
                }

            };
            
            _mockUserTokenRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<UserToken>()))
                .ReturnsAsync(userToken);

            _mockUserRepository.Setup(repo => repo.Get()).
                Returns(users.AsQueryable().BuildMockDbSet().Object);

           var actual = await _testing.AuthenticateAsync(username, password);

            Assert.NotEqual(expiredUserToken.Token, actual.Token);
            Assert.Equal(userToken.Token, actual.Token);

            _mockUserTokenRepository.Verify(token =>
                    token.UpdateAsync(It.IsAny<UserToken>()),
                    Times.Once);
        }

        [Fact]
        public async Task AuthenticateAsync_UserNameIsNull_ThrowsArgumentNullException()
        {
            string username = null;
            string password = "Test";

            async Task Test() => await _testing.AuthenticateAsync(username, password);

            await Assert.ThrowsAsync<ArgumentNullException>(Test);
        }

        [Fact]
        public async Task AuthenticateAsync_PasswordIsNull_ThrowsArgumentNullException()
        {
            string username = "Test";
            string password = null;

            async Task Test() => await _testing.AuthenticateAsync(username, password);

            await Assert.ThrowsAsync<ArgumentNullException>(Test);
        }

    }
}
