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
            var user = new UserService();
            var actual = user.Encrypt("Meryem", "mzbasel");
            Assert.Equal(actual, user.Encrypt("Meryem", "mzbasel"));
            
        }
    }
}
