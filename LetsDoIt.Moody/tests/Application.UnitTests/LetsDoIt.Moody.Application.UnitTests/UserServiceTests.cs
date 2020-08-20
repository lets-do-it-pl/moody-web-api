using Moq;
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Authentication;

namespace LetsDoIt.Moody.Application.UnitTests
{
    using Domain;
    using Application.User;
    using Persistance.Repositories.Base;

    public class UserServiceTests
    {
        private readonly Mock<IEntityRepository<UserToken>> _mockUserTokenRepository;
        private readonly Mock<IEntityRepository<User>> _mockUserRepository;
        private readonly IUserService _testing;
        private readonly string _applicationKey = "something";
        private readonly int _tokenExpirationMinutes = 123;

        public UserServiceTests()
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
            var username = "Test";
            var userpassword = "12345";
            var token = "jhafusfcuyfeuycewhvbcbhec";

            var user = new List<User>
            {
                new User()
                {
                    UserName = username,
                    Password = userpassword
                }

            };

            var userToken = new List<UserToken>
            {
                new UserToken()
                {
                    Token = token
                }
            };


            _mockUserRepository.Setup(repo => repo.Get()).Returns(user.AsQueryable());
            _mockUserTokenRepository.Setup(token => token.Get()).Returns(userToken.AsQueryable());

            var actual = await _testing.AuthenticateAsync(username, userpassword);
            Assert.Equal(actual.Token , token);

        }

        [Fact]
        public async Task AuthenticateAsync_UserDoesNotExistsInTheDatabase_ThrowAuthenticationException()
        {
            var username = "Test";
            var userpassword = "12345";

            async Task Test() => await _testing.AuthenticateAsync(username, userpassword);

            Assert.ThrowsAsync<AuthenticationException>(Test);
        }

        [Fact]
        public async Task AuthenticateAsync_UserExistsWithoutToken_ShouldGenerateAToken()
        {
            var username = "Test";
            var userpassword = "12345";

            var user = new List<User>
            {
                new User()
                {
                    UserName = username,
                    Password = userpassword
                }

            };

            var userTokens = new List<UserToken>
            {
                new UserToken()
                {
                    Token = null
                }
            };

            _mockUserRepository.Setup(user => user.Get()).Returns(user.AsQueryable());
            _mockUserTokenRepository.Setup(token => token.Get()).Returns(userTokens.AsQueryable());

            var actual = await _testing.AuthenticateAsync(username, userpassword);

            _mockUserTokenRepository.Verify(token =>
                    token.UpdateAsync(It.Is<UserToken>(x => x.Token == actual.Token)),
                    Times.Once);
        }

        [Fact]
        public async Task AuthenticateAsync_UserExistsWithExpiredToken_ShouldGenerateNewToken()
        {
            var username = "Test";
            var userpassword = "12345";
            var userToken = "jhafusfcuyfeuycewhvbcbhec";

            var user = new List<User>
            {
                new User()
                {
                    UserName = username,
                    Password = userpassword
                }

            };

            var userTokens = new List<UserToken>
            {
                new UserToken()
                {
                    Token = userToken,
                    ExpirationDate = DateTime.Now
                }
            };


            _mockUserRepository.Setup(repo => repo.Get()).Returns(user.AsQueryable());
            _mockUserTokenRepository.Setup(token => token.Get()).Returns(userTokens.AsQueryable());

            var actual = await _testing.AuthenticateAsync(username, userpassword);

            _mockUserTokenRepository.Verify(token =>
                    token.UpdateAsync(It.Is<UserToken>(x => x.Token == actual.Token)),
                    Times.Once);
        }

    }
}
