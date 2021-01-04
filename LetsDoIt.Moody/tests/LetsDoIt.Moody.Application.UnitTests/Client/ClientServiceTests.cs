using LetsDoIt.Moody.Application.Client;
using LetsDoIt.Moody.Application.Security;
using Moq;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Security.Authentication;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests.Client
{
    using Constants;
    using Persistence.Repositories.Base;
    using Persistence.Entities;

    public class ClientServiceTests
    {
        private readonly IClientService _testing;
        private readonly Mock<IRepository<Client>> _mockClientRepository;
        private readonly Mock<ISecurityService> _mockSecurityService;

        public ClientServiceTests()
        {
            _mockClientRepository = new Mock<IRepository<Client>>();
            _mockSecurityService = new Mock<ISecurityService>();

            _testing = new ClientService
                (_mockClientRepository.Object,
                _mockSecurityService.Object);
        }

        #region SaveClientAsync

        [Theory]
        [InlineData(null, "pass")]
        [InlineData("filler", null)]
        public async Task SaveClientAsync_WhenUserNameOrPasswordIsNull_ShouldThrowArgumentNullException(string username, string password)
        {
            async Task Test() => await _testing.SaveClientAsync(username, password);

            await Assert.ThrowsAsync<ArgumentNullException>(Test);
        }

        [Theory]
        [InlineData("", "pass")]
        [InlineData(" ", "pass")]
        [InlineData("filler", "")]
        [InlineData("filler", " ")]
        public async Task SaveClientAsync_WhenPasswordOrUserNameEmptyOrWhiteSpace_ShouldThrowArgumentException(string userName, string password)
        {
            async Task Test() => await _testing.SaveClientAsync(userName, password);

            await Assert.ThrowsAsync<ArgumentException>(Test);
        }

        [Fact]
        public async Task SaveClientAsync_WhenClientAlreadyExists_ShouldThrowDuplicateNameException()
        {
            var username = "filler";
            var password = "pass";

            _mockClientRepository.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Client, bool>>>()))
                .ReturnsAsync(true);

            async Task Test() => await _testing.SaveClientAsync(username, password);

            await Assert.ThrowsAsync<DuplicateNameException>(Test);
        }

        [Fact]
        public async Task SaveClientAsync_GivenNoException_ShouldInvokeClientRepositoryAddAsync()
        {
            var user = new Client
            {
                Username = "asd",
                Password = "pass"
            };

            await _testing.SaveClientAsync(user.Username, user.Password);

            _mockClientRepository.Verify(ur =>
                    ur.AddAsync(It.Is<Client>(x => x.Username == user.Username)),
                Times.Once);
        }
        #endregion

        #region AuthenticateAsync

        [Fact]
        public async Task AuthenticateAsync_WhenClientExists_ShouldReturnJwtToken()
        {
            var id = 1;
            var username = "good.username";
            var password = "good.password";
            var token = "good.token";

            _mockClientRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Client, bool>>>()))
                .ReturnsAsync(new Client
                {
                    Id = id,
                    Username = username,
                    Password = password
                });

            _mockSecurityService.Setup(ss => ss.GenerateJwtToken(id.ToString(), username, UserTypeConstants.Client))
                .Returns(new TokenInfo(token,DateTime.UtcNow.AddMinutes(1440)));


            var actual = await _testing.AuthenticateAsync(username, password);

            Assert.NotNull(actual);
            Assert.NotNull(actual.Token);
            Assert.Equal(username, actual.Username);
            Assert.Equal(token, actual.Token);
            Assert.True(DateTime.UtcNow < actual.ExpirationDate);
        }


        [Fact]
        public async Task AuthenticateAsync_WhenClientDoesNotExists_ShouldThrowAuthenticationException()
        {
            var username = "bad.username";
            var password = "bad.password";

            _mockClientRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Client, bool>>>()))
                .ReturnsAsync((Client)null);

            async Task Test() => await _testing.AuthenticateAsync(username, password);

            await Assert.ThrowsAsync<AuthenticationException>(Test);
        }

        [Theory]
        [InlineData(null, "pass")]
        [InlineData("filler", null)]
        public async Task AuthenticateAsync_UserNameIsNull_ThrowsArgumentNullException(string username, string password)
        {

            async Task Test() => await _testing.AuthenticateAsync(username, password);

            await Assert.ThrowsAsync<ArgumentNullException>(Test);
        }
        #endregion
    }
}
