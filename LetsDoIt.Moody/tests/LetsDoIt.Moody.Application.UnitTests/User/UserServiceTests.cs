using System;
using LetsDoIt.MailSender;
using LetsDoIt.Moody.Application.Security;
using LetsDoIt.Moody.Application.User;
using Moq;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests.User
{
    using CustomExceptions;
    using Infrastructure.Utils;
    using Persistence.Repositories.Base;
    using Persistence.Entities;

    public class UserServiceTests
    {
        private readonly IUserService _testing;
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IMailSender> _mockMailSender;
        private readonly Mock<ISecurityService> _mockSecurityService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockMailSender = new Mock<IMailSender>();
            _mockSecurityService = new Mock<ISecurityService>();

            _testing = new UserService(_mockUserRepository.Object, _mockMailSender.Object, _mockSecurityService.Object);
        }

        #region SaveUserAsync

        [Fact]
        public async Task SaveUserAsync_ShouldCallUserRepositoryWithGivenParameters()
        {
            var username = "good.username";
            var password = "good.password";
            var email = "good.email";
            var name = "good.name";
            var surname = "good.surname";

            await _testing.SaveUserAsync(username, password, email, name, surname);

            _mockUserRepository.Verify(ur => ur.AddAsync(It.Is<User>(u => u.Username == username &&
                                                                         u.Password == ProtectionHelper.EncryptValue(username + password) &&
                                                                         u.Email == email &&
                                                                         u.FullName == name + " " + surname)), Times.Once);
        }

        [Fact]
        public async Task SaveUserAsync_ShouldThrowDuplicateNameException_WhenUsernameOrEmailAlreadyExistInDatabase()
        {
            var username = "bad.username";
            var password = "good.password";
            var email = "bad.email";
            var name = "good.name";
            var surname = "good.surname";

            _mockUserRepository.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(true);

            async Task Test() => await _testing.SaveUserAsync(username, password, email, name, surname);

            await Assert.ThrowsAsync<DuplicateNameException>(Test);
        }
        #endregion

        #region SendActivationEmailAsync

        [Fact]
        public async Task SendActivationEmailAsync_ShouldThrowEmailNotRegisteredException_WhenEmailNotRegistered()
        {
            var email = "bad.email";

            _mockUserRepository.Setup(ur => ur.GetAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync((User)null);

            async Task Test() => await _testing.SendActivationEmailAsync("good.referer", email);

            await Assert.ThrowsAsync<EmailNotRegisteredException>(Test);
        }

        [Fact]
        public async Task SendActivationEmailAsync_ShouldTCallSecurityServiceGenerateJwtTokenAndIMailSender()
        {
            var email = "good.email";

            _mockUserRepository.Setup(ur => ur.GetAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(new User
            {
                Id = 1,
                FullName = "Full Name"
            });

            _mockSecurityService.Setup(ss => ss.GenerateJwtToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).Returns(new TokenInfo("good.token", DateTime.MaxValue));

            await _testing.SendActivationEmailAsync("https://developer.mozilla.org/en-US/docs/Web/JavaScript", email);

            _mockSecurityService.Verify(ss => ss.GenerateJwtToken("1", "Full Name", It.IsAny<string>()), Times.Once);

            _mockMailSender.Verify(ms => ms.SendAsync(It.IsAny<string>(), It.IsAny<string>(), email), Times.Once);
        }
        #endregion

        #region ActivateUserAsync

        [Fact]
        public async Task ActivateUserAsync_ShouldActivateUserAndUpdateUserByCallingUserRepository()
        {
            var id = 1;

            _mockUserRepository.Setup(ur =>
                ur.GetAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(new User
                {
                    Id = id
                });

            await _testing.ActivateUserAsync(id);


            _mockUserRepository.Verify(ur => ur.UpdateAsync(It.Is<User>(u => u.Id == id && u.IsActive)), Times.Once);
        }

        [Fact]
        public async Task ActivateUserAsync_ShouldThrowUserNotFoundException_WhenUserNotRegistered()
        {
            var id = 1;

            _mockUserRepository.Setup(ur => ur.GetAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync((User)null);

            async Task Test() => await _testing.ActivateUserAsync(id);

            await Assert.ThrowsAsync<UserNotFoundException>(Test);
        }
        #endregion
    }
}
