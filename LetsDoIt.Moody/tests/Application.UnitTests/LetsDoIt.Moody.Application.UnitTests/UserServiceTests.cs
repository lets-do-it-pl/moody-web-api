using Moq;
using Xunit;
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
            _testing = new UserService(_mockUserRepository.Object, _mockUserTokenRepository.Object, _applicationKey, _tokenExpirationMinutes);
        }

        [Fact]
        public async Task AuthenticateAsync_UserExistsAndTokenIsNotNull_ReturnToken()
        {
            var username = "Test";
            var userpassword = "12345";
            var token = "sakfhkgfsgfygsakjghgguy";

            var user = new List<User>
            {
                new User()
                {
                    UserName = username,
                    Password = userpassword
                }

            }.AsQueryable();

            var userToken = new List<UserToken>
            {
                new UserToken()
                {
                    Token = token
                }
            }.AsQueryable();


            _mockUserRepository.Setup(repo => repo.Get()).Returns(user);
            _mockUserTokenRepository.Setup(token => token.Get()).Returns(userToken);

            var actual = await _testing.AuthenticateAsync(username, userpassword);
            Assert.IsType<UserToken>(actual.Token);
        }

        [Fact]
        public async Task AuthenticateAsync_UserDoesNotExistsInTheDatabase_ThrowAuthenticationException()
        {
            var username = "Test";
            var userpassword = "12345";

            var user = new List<User>
            {
                new User()
                {
                    UserName = null,
                    Password = null
                }
            }.AsQueryable();

            _mockUserRepository.Setup(repo => repo.Get()).Returns(user);

            Task Test() =>  _testing.AuthenticateAsync(username, userpassword);

            await Assert.ThrowsAsync<AuthenticationException>(Test);
        }

        [Fact]
        public async Task AuthenticateAsync_UserExistsWithoutToken_ShouldGenerateAToken()
        {
            
        }

        [Fact]
        public async Task AuthenticateAsync_UserExistsWithExpiredToken_ShouldGenerateNewToken()
        {

        }

    }
}
