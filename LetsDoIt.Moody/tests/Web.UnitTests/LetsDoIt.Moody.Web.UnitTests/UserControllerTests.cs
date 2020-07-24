using System;
using System.Collections.Generic;
using System.Text;
using LetsDoIt.Moody.Application.Services.UserFolder;
using LetsDoIt.Moody.Web.Controllers;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Web.UnitTests
{
    public class UserControllerTests
    {
        private readonly UserController _testing;
        private readonly Mock<IUserService> _mockUserService;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _testing = new UserController(_mockUserService.Object);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void SaveUser_WhenUserNameIsMissing_ShouldThrownAnArgumentException(string userName)
        {
            // act 
            Action action = () => _testing.SaveUser(userName,"asd123");

            //assert
            Assert.Throws<ArgumentException>(action);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void SaveUser_WhenPasswordIsMissing_ShouldThrownAnArgumentException(string password)
        {
            // act 
            Action action = () => _testing.SaveUser("Kukilamasoliksanae",password);

            //assert
            Assert.Throws<ArgumentException>(action);
        }


        [Fact]
        public void SaveUser_ShouldSaveUserInformation()
        {
            // arrange
            var userName = "Deneme";
            var password = "asdasd12321";

            _mockUserService.Setup(cs => cs.SaveUser(userName,password));

            // act
            _testing.SaveUser(userName,password);

            // assert
            _mockUserService.Verify(cs => cs.SaveUser(userName,password));
        }
    }
}
