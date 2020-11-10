using Moq;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Data;
using System.Net;
using Castle.Core.Logging;
using LetsDoIt.Moody.Domain;
using LetsDoIt.Moody.Web.Controllers;
using Microsoft.Extensions.Logging;

namespace LetsDoIt.Moody.Web.UnitTests.Controllers
{
    using Application.User;
    using Entities.Requests;
    public class UserControllerTests
    {
        private readonly Mock<ILogger<UserController>> _mockLogger;
        private readonly UserController _testing;
        private readonly Mock<IUserService> _mockUserService;
        public UserControllerTests()
        {
            _mockLogger = new Mock<ILogger<UserController>>();
            _mockUserService = new Mock<IUserService>();
            _testing = new UserController(_mockUserService.Object, _mockLogger.Object);
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

            _mockUserService.Setup(user => user.AuthenticateAsync(username, userpassword));

            var response = await _testing.Authenticate(username, userpassword);

            Assert.IsType<OkObjectResult>(response.Result);
        }

        [Fact]
        public async Task SaveUser_WhenDuplicateNameExceptionThrown_ShouldReturnBadRequest()
        {
             _mockUserService.Setup(x =>
                 x.SaveUserAsync(It.IsAny<string>(), It.IsAny<string>())).Throws(new DuplicateNameException());

            var actual = await _testing.SaveUser(new SaveUserRequest
            {
                Username = "usernamefiller",
                Password = "passwordfiller",
                Name = "namefiller",
                Email = "emailfiller",
                Surname = "surnamefiller",
                IsActive = false
            });

            Assert.IsType<BadRequestObjectResult>(actual);
        }

        [Fact]
        public async Task SaveUser_ShouldSaveUserInformationAndReturnOk()
        {
            var saveUserRequest = new SaveUserRequest()
            {
                Username = "test", 
                Password= "test"
            };

          var response =await _testing.SaveUser(saveUserRequest);

          Assert.IsType<ObjectResult>(response);

          var objectResponse = response as ObjectResult; //Cast to desired type

          Assert.Equal((int)HttpStatusCode.Created, objectResponse.StatusCode);
        }

        [Fact]
       public async Task SaveUser_ShouldInvokeUserServiceSaveUserAsync()
        {
            var saveUserRequest = new SaveUserRequest()
            {
                Username = "test",
                Password = "test"
            };

            await _testing.SaveUser(saveUserRequest);

            _mockUserService.Verify(us =>
                    us.SaveUserAsync(
                        saveUserRequest.Username,
                        saveUserRequest.Password
                    ),
                Times.Once);
        }

    }
}
