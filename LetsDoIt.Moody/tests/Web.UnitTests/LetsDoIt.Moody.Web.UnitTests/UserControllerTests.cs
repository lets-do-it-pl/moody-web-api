using Moq;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
