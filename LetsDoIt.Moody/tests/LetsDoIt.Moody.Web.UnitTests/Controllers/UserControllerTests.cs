using Moq;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Data;
using System.Net;
using LetsDoIt.Moody.Application.Client;
using LetsDoIt.Moody.Web.Controllers;
using LetsDoIt.Moody.Web.Entities.Requests.Client;
using Microsoft.Extensions.Logging;

namespace LetsDoIt.Moody.Web.UnitTests.Controllers
{
    using Entities.Requests;
    public class UserControllerTests
    {
        private readonly ClientController _testing;
        private readonly Mock<IClientService> _mockUserService;
        public UserControllerTests()
        {
            _mockUserService = new Mock<IClientService>();
            _testing = new ClientController(_mockUserService.Object);
        }

        [Fact]
        public async Task SaveUser_WhenDuplicateNameExceptionThrown_ShouldReturnBadRequest()
        {
            _mockUserService.Setup(x =>
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

            _mockUserService.Verify(us =>
                    us.SaveClientAsync(
                        saveUserRequest.Username,
                        saveUserRequest.Password
                    ),
                Times.Once);
        }
    }
}
