using Moq;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace LetsDoIt.Moody.Web.UnitTests
{
    using Application.User;
    using Web.Controllers;

    public class UserControllerTests
    {
        private readonly UserController _testing;
        private readonly Mock<IUserService> _mockUserService;
        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _testing = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_UserDoesNotExistsInTheDatabase_ReturnsBadRequest()
        {
            var username = "Test";
            var userpassword = "12345";

            _mockUserService.Setup(user =>
                user.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>())).Throws(new AuthenticationException());

            var actual = await _testing.Authenticate(username, userpassword);
            Assert.IsType<BadRequestObjectResult>(actual.Result);
        }

        [Fact]
        public async Task AuthenticateAsync_UserDoesExistsInTheDatabase_ReturnsOk()
        {
            var username = "Test";
            var userpassword = "12345";

            var response = await _testing.Authenticate(username, userpassword);

            Assert.IsType<OkObjectResult>(response.Result);
        }
    }
}
