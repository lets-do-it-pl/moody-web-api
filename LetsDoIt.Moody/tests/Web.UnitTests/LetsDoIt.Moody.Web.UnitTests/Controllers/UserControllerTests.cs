using Moq;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Data;
using System.Net;
using LetsDoIt.Moody.Domain;
using LetsDoIt.Moody.Web.Controllers;
using Microsoft.Extensions.Logging;

namespace LetsDoIt.Moody.Web.UnitTests.Controllers
{
    using Application.User;
    using Entities.Requests;
    public class ClientControllerTests
    {
        private readonly Mock<ILogger<ClientController>> _mockLogger;
        private readonly ClientController _testing;
        private readonly Mock<IUserService> _mockUserService;
        public ClientControllerTests()
        {
            _mockLogger = new Mock<ILogger<ClientController>>();
            _mockUserService = new Mock<IUserService>();
            _testing = new ClientController(_mockUserService.Object, _mockLogger.Object);
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
        public async Task SaveClient_WhenDuplicateNameExceptionThrown_ShouldReturnBadRequest()
        {
            _mockUserService.Setup(x =>
                x.SaveUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<UserTypes>(),
                    It.IsAny<string>(), It.IsAny<string>(),It.IsAny<string>())).Throws(new DuplicateNameException());

            var actual = await _testing.SaveClient(new SaveClientRequest
            {
                Username = "usernamefiller",
                Password = "passwordfiller"
            });

            Assert.IsType<BadRequestObjectResult>(actual);
        }

        [Fact]
        public async Task SaveClient_ShouldSaveUserInformationAndReturnOk()
        {
            var saveUserRequest = new SaveClientRequest()
            {
                Username = "test",
                Password = "test"
            };

            var response = await _testing.SaveClient(saveUserRequest);

            Assert.IsType<ObjectResult>(response);

            var objectResponse = response as ObjectResult; //Cast to desired type

            Assert.Equal((int)HttpStatusCode.Created, objectResponse.StatusCode);
        }

        [Fact]
        public async Task SaveClient_ShouldInvokeUserServiceSaveUserAsync()
        {
            var saveUserRequest = new SaveClientRequest()
            {
                Username = "test",
                Password = "test",
                IsActive = true
            };

            await _testing.SaveClient(saveUserRequest);

            _mockUserService.Verify(us =>
                    us.SaveUserAsync(
                        saveUserRequest.Username,
                        saveUserRequest.Password,
                        saveUserRequest.IsActive,
                        UserTypes.Mobile,
                        null,
                        null,
                        null
                    ),
                Times.Once);
        }

    }
}
