using System;
using System.Collections.Generic;
using System.Text;
using LetsDoIt.Moody.Application.Category;
using LetsDoIt.Moody.Application.User;
using LetsDoIt.Moody.Web.Controllers;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Web.UnitTests
{
    public class UserControllerTests
    {
        private readonly UserController _testing;

        public UserControllerTests()
        {
            var mockUserService = new Mock<IUserService>();
            _testing = new UserController(mockUserService.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void SaveUserAsync_WhenUserNameIsMissing_ShouldThrownAnArgumentException(string userName)
        {
            // act 
            Action action = async () => await _testing.SaveUserAsync(userName,"asdfgh");

            //assert
            Assert.Throws<ArgumentException>(action);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void SaveUserAsync_WhenPasswordIsMissing_ShouldThrownAnArgumentException(string password)
        {
            // act 
            Action action = async () => await _testing.SaveUserAsync("ben", password);

            //assert
            Assert.Throws<ArgumentException>(action);
        }


       
    }
}
