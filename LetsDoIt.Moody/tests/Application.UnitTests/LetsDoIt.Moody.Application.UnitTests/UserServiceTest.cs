using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests
{
    using LetsDoIt.Moody.Application.Services;

    public class UserServiceTest
    {
        [Fact]
        public void UserSevice_WhetherEncryptedProperly_ReturnEncryptedText()
        {
            var user = new UserService("Meryem", "12345");
            var actual = user.EncryptUserNameAndPassword();
            Assert.Equal(actual, new UserService("Meryem", "12344").EncryptUserNameAndPassword());
            Assert.NotEqual(actual, new UserService("Zeynep", "12345").EncryptUserNameAndPassword());

        }

        [Fact]
        public void UserService_Authorize_ReturnToken()
        {


        }
    }
}
