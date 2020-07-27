using Xunit;
using Moq;
using LetsDoIt.Moody.Web.Controllers;
using LetsDoIt.Moody.Application.Services;

namespace LetsDoIt.Moody.Application.UnitTests
{
    public class UserControllerTest
    {
       
        private readonly UserController _testing;
        private readonly Mock<IUserService> _mockUserService;

        public UserControllerTest()
        {
            _mockUserService = new Mock<IUserService>();
            _testing = new UserController(_mockUserService.Object);
        }

        [Fact]
        public void UserController_AuthorizeUser()
        {
            var username = "John";
            var password = "12345";

            _mockUserService.Setup(cs => cs.Authenticate(cs.EncryptUserNameAndPassword(username, password)));

            // act
            _testing.Authenticate(username, password);

            // assert
            _mockUserService.Verify(cs => cs.Authenticate(cs.EncryptUserNameAndPassword(username, password)));
        }
        
    }
}
