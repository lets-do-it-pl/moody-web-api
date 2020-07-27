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
            encryptUserMock.Setup(x => x.EncryptUserNameAndPassword("test1", "password1")).Returns("4w3A6H263XZQGo1hFaAciFdiQg/nTxSeWhANED2PA5Q=");
            encryptUserMock.Verify(cs => cs.EncryptUserNameAndPassword("test1", "password1"));
        }

    }
}
