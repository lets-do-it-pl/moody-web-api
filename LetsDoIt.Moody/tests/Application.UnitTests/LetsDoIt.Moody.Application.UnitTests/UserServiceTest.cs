using LetsDoIt.Moody.Application.Services;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests
{

    public class UserServiceTest
    {
        [Fact]
        public void UserSevice_WhetherEncryptedProperly_ReturnEncryptedText()
        { 
            var encryptUserMock = new Mock<UserService>();
            var expectedUserMock = new Mock<UserService>();
            encryptUserMock.Setup(x => x.EncryptUserNameAndPassword("John", "12345"));
            encryptUserMock.Verify(cs => cs.EncryptUserNameAndPassword("John", "12345"));
        }

    }
}
