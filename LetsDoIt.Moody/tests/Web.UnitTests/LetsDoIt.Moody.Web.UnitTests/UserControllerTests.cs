using Moq;
using Xunit;

namespace LetsDoIt.Moody.Web.UnitTests
{
    using System.Net;
    using System.Threading.Tasks;
    using LetsDoIt.Moody.Application.User;
    using LetsDoIt.Moody.Web.Controllers;
    using LetsDoIt.Moody.Web.Entities.Requests;
    using Microsoft.AspNetCore.Mvc;

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


        }

        [Fact]
        public async Task AuthenticateAsync_UserDoesExistsInTheDatabase_ReturnsOkWithToken()
        {
            var username = "Meryem";
            var userpassword = "12345";

            _mockUserService.Setup(user => user.AuthenticateAsync(username, userpassword));

            var response = await _testing.Authenticate(username, userpassword);

            Assert.IsType<OkObjectResult>(response.Result);
        }
    }
}
