using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests
{
    using LetsDoIt.Moody.Application.Services;
    using Moq;

    public class UserServiceTest
    {
        /*private readonly UserService _testing;
        private readonly Mock<IUserService> _mockUserService;

        public UserServiceTest()
        {
            _mockUserService = new Mock<IUserService>();
            _testing = new UserService(_mockUserService.Object);
        }*/

       [Fact]
       public void UserSevice_WhetherEncryptedProperly_ReturnEncryptedText ()
        {
            var user = new UserService("Meryem", "12345");
            var actual = user.EncryptUserNameAndPassword();
            Assert.Equal(actual, new UserService("Meryem", "12345").EncryptUserNameAndPassword());
            Assert.NotEqual(actual, new UserService("Zeynep", "12345").EncryptUserNameAndPassword());
            
        }
    }
}
