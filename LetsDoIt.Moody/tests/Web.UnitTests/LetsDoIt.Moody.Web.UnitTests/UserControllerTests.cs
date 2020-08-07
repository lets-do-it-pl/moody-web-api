//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using LetsDoIt.Moody.Application.Category;
//using LetsDoIt.Moody.Application.User;
//using LetsDoIt.Moody.Web.Controllers;
//using Moq;
//using Xunit;

//namespace LetsDoIt.Moody.Web.UnitTests
//{
//    public class UserControllerTests
//    {
//        private readonly UserController _testing;
//        private readonly Mock<IUserService> _mockUserService;
//        public UserControllerTests()
//        {
//            _mockUserService = new Mock<IUserService>();
//            _testing = new UserController(_mockUserService.Object);
//        }

//        [Theory]
//        [InlineData(null)]
//        [InlineData("")]
//        [InlineData(" ")]
//        public async Task SaveUserAsync_WhenUserNameIsMissing_ShouldThrownAnArgumentException(string userName)
//        {
//            // act 
//            async Task Action() => await _testing.SaveUser(userName, "asdfgh");

//            //assert
//            await Assert.ThrowsAsync<ArgumentException>(Action);
//        }

//        [Theory]
//        [InlineData(null)]
//        [InlineData("")]
//        [InlineData(" ")]
//        public async Task SaveUserAsync_WhenPasswordIsMissing_ShouldThrownAnArgumentException(string password)
//        {
//            async Task Action() => await _testing.SaveUser("ben", password);

//            //assert
//            await Assert.ThrowsAsync<ArgumentException>(Action);

//        }

//        [Fact]
//        public async Task SaveUserAsync_ShouldSaveUserInformation()
//        {
//            // arrange
//            var userName = "Deneme";
//            var password = "asdfgh";

//            _mockUserService.Setup(us => us.SaveUserAsync(userName,password));

//            // act
//            await _testing.SaveUser(userName, password);

//            // assert
//            _mockUserService.Verify(us => us.SaveUserAsync(userName,password),Times.Once);
//        }



//    }
//}
