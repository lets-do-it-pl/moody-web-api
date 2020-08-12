using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.User;
using LetsDoIt.Moody.Domain;
using LetsDoIt.Moody.Web.Controllers;
using LetsDoIt.Moody.Web.Entities.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Web.UnitTests
{
    public class UserControllerTests
    {
        private readonly UserController _sutUserController;
        private readonly Mock<IUserService> _mockUserService;
        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _sutUserController = new UserController(_mockUserService.Object);
        }

        //[Theory]
        //[InlineData(null)]
        //[InlineData("")]
        //[InlineData(" ")]
        //public async Task SaveUserAsync_WhenUserNameIsMissing_ShouldThrownAnArgumentException(string userName)
        //{
        //    // act 
        //    async Task Action() => await _sutUserController.SaveUser(userName, "asdfgh");

        //    //assert
        //    await Assert.ThrowsAsync<ArgumentException>(Action);
        //}

        //[Theory]
        //[InlineData(null)]
        //[InlineData("")]
        //[InlineData(" ")]
        //public async Task SaveUserAsync_WhenPasswordIsMissing_ShouldThrownAnArgumentException(string password)
        //{
        //    async Task Action() => await _sutUserController.SaveUser("ben", password);

        //    //assert
        //    await Assert.ThrowsAsync<ArgumentException>(Action);

        //}

        //[Fact]
        //public async Task SaveUserAsync_ShouldSaveUserInformation()
        //{
        //    // arrange
        //    var userName = "Deneme";
        //    var password = "asdfgh";

        //    _mockUserService.Setup(us => us.SaveUserAsync(userName, password));

        //    // act
        //    await _sutUserController.SaveUser(userName, password);

        //    // assert
        //    _mockUserService.Verify(us => us.SaveUserAsync(userName, password), Times.Once);
        //}

        
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

    }
}
