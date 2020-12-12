using System.Data;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.Client;
using LetsDoIt.Moody.Web.Controllers;
using LetsDoIt.Moody.Web.Entities.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Web.UnitTests.Controllers
{
    public class ClientControllerTests
    {
        private readonly ClientController _testing;
        private readonly Mock<IClientService> _mockClientService;

        public ClientControllerTests()
        {
            _mockClientService = new Mock<IClientService>();
            _testing = new ClientController(_mockClientService.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_UserDoesNotExistsInTheDatabase_ReturnsBadRequest()
        {
            var username = "Test";
            var userpassword = "12345";

            _mockClientService.Setup(user =>
                user.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>())).Throws(new AuthenticationException());

            var actual = await _testing.Authenticate(username, userpassword);
            Assert.IsType<BadRequestObjectResult>(actual.Result);
        }

        [Fact]
        public async Task AuthenticateAsync_UserDoesExistsInTheDatabase_ReturnsOk()
        {
            var username = "Test";
            var userpassword = "12345";

            _mockClientService.Setup(user => user.AuthenticateAsync(username, userpassword));

            var response = await _testing.Authenticate(username, userpassword);

            Assert.IsType<OkObjectResult>(response.Result);
        }

        [Fact]
        public async Task SaveUser_WhenDuplicateNameExceptionThrown_ShouldReturnBadRequest()
        {
            _mockClientService.Setup(x =>
                x.SaveClientAsync(It.IsAny<string>(), It.IsAny<string>())).Throws(new DuplicateNameException());

            var actual = await _testing.SaveClient(new SaveClientRequest
            {
                Username = "usernamefiller",
                Password = "passwordfiller"
            });

            Assert.IsType<BadRequestObjectResult>(actual);
        }

        [Fact]
        public async Task SaveUser_ShouldSaveUserInformationAndReturnOk()
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
        public async Task SaveUser_ShouldInvokeUserServiceSaveUserAsync()
        {
            var saveUserRequest = new SaveClientRequest()
            {
                Username = "test",
                Password = "test"
            };

            await _testing.SaveClient(saveUserRequest);

            _mockClientService.Verify(us =>
                    us.SaveClientAsync(
                        saveUserRequest.Username,
                        saveUserRequest.Password
                    ),
                Times.Once);
        }
    }
}
