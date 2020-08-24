<<<<<<< HEAD
﻿using Moq;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace LetsDoIt.Moody.Web.UnitTests
{
    using Application.User;
    using Web.Controllers;

=======
using System.Data;
using System.Net;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.User;
using LetsDoIt.Moody.Web.Controllers;
using LetsDoIt.Moody.Web.Entities.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Web.UnitTests.Controllers
{
>>>>>>> 81c6bfc63c952b4dc89c09dfabf14f031cb67670
    public class UserControllerTests
    {
        private readonly UserController _testing;
        private readonly Mock<IUserService> _mockUserService;
        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _testing = new UserController(_mockUserService.Object);
        }

<<<<<<< HEAD
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
=======

        [Fact]
        public async Task SaveUser_WhenDuplicateNameExceptionThrown_ShouldReturnBadRequest()
        {
            _mockUserService.Setup(x =>
                x.SaveUserAsync(It.IsAny<string>(),It.IsAny<string>())).Throws(new DuplicateNameException());

            var actual = await _testing.SaveUser(new SaveUserRequest
            {
                Username = "usernamefiller",
                Password = "passwordfiller"
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

>>>>>>> 81c6bfc63c952b4dc89c09dfabf14f031cb67670
    }
}
