using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Web.UnitTests
{
    using Application.User;
    using Controllers;
    using Entities.Requests;
    public class UserControllerTests
    {
        private readonly UserController _sutUserController;
        private readonly Mock<IUserService> _mockUserService;
        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _sutUserController = new UserController(_mockUserService.Object);
        }

        [Fact]
        public void SaveUserRequestUserName_ShouldHaveRequiredAttribute()
        {
           Assert.NotNull(typeof(SaveUserRequest).GetProperty("Username").GetCustomAttribute<RequiredAttribute>());
        }

        [Fact]
        public void SaveUserRequestPassword_ShouldHaveRequiredAttribute()
        {
            Assert.NotNull(typeof(SaveUserRequest).GetProperty("Password").GetCustomAttribute<RequiredAttribute>());
        }


        [Fact]
        public async Task SaveUser_WhenDuplicateNameExceptionThrown_ShouldReturnBadRequest()
        {
            _mockUserService.Setup(x =>
                x.SaveUserAsync(It.IsAny<string>(),It.IsAny<string>())).Throws(new DuplicateNameException());

            IActionResult actual = await _sutUserController.SaveUser(new SaveUserRequest
            {
                Username = "usernamefiller",
                Password = "passwordfiller"
            });

            Assert.IsAssignableFrom<BadRequestObjectResult>(actual);
        }

        [Fact]
        public async Task SaveUser_ShouldSaveUserInformationAndReturnOk()
        {
            var saveUserRequest = new SaveUserRequest()
            {
                Username = "test", 
                Password= "test"
            };

          var response =await _sutUserController.SaveUser(saveUserRequest);

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

            await _sutUserController.SaveUser(saveUserRequest);

            _mockUserService.Verify(us =>
                    us.SaveUserAsync(
                        saveUserRequest.Username,
                        saveUserRequest.Password
                    ),
                Times.Once);
        }

    }
}
    