using Moq;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests
{
    using LetsDoIt.Moody.Application.User;
    using LetsDoIt.Moody.Persistance.Repositories.Base;
    using LetsDoIt.Moody.Domain;
    using System.Threading.Tasks;

    public class UserServiceTests
    {
        private readonly UserService userService;
        private readonly Mock<IUserService> userServiceMock;
        private readonly Mock<IEntityRepository<User>> entityRepositoryUserMock;
        private readonly Mock<IEntityRepository<UserToken>> entityRepositoryTokenMock;

        public UserServiceTests()
        {
            var applicationKey = "uwgeiwgdjhqvfwjhqvfluG";
            var tokenExpirationMinutes = 12;
            entityRepositoryUserMock = new Mock<IEntityRepository<User>>();
            entityRepositoryTokenMock = new Mock<IEntityRepository<UserToken>>();
            userServiceMock = new Mock<IUserService>();

            userService = new UserService((IEntityRepository<User>)entityRepositoryUserMock, (IEntityRepository<UserToken>)entityRepositoryTokenMock, applicationKey, tokenExpirationMinutes);
        }

        [Fact(Skip = "Broken")]
        public async Task AuthenticationAsync_UserExists_ReturnToken()
        {
            //Arrange
            var token = "uhsaiufhisgefgbfuiwafhenafiohu";
            var username = "Meryem";
            var userpassword = "12345";
            var userTokenEntity = new UserTokenEntity
            {
                Username = username,
                Token = token
            };
            userServiceMock.Setup(u => u.AuthenticateAsync(username, userpassword)).ReturnsAsync(userTokenEntity);

            //Act
            var user = await userService.AuthenticateAsync(username, userpassword);

            //Assert
            Assert.Equal(username, user.Username);
            Assert.Equal(token, user.Token);
        }
        
    }
}
