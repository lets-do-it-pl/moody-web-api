using System;
using System.Data;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.User;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests
{
    using Domain;
    using Persistance.Repositories.Base;
    using System.Linq;

    public class UserSeviceTests
    {
        private readonly Mock<IEntityRepository<UserToken>> _mockUserTokenRepository;
        private Mock<IEntityRepository<User>> _mockUserRepository;
        private readonly IUserService _sutUserService;

        public UserSeviceTests()
        {
            _mockUserRepository = new Mock<IEntityRepository<User>>();
            _mockUserTokenRepository=new Mock<IEntityRepository<UserToken>>();
            _sutUserService=new UserService(_mockUserRepository.Object,_mockUserTokenRepository.Object, "deneme", 12321);
        }

        [Theory]
        [InlineData(null)]
        public async Task SaveUserAsync_WhenUserNameIsMissing_ShouldThrowAnException(string userName)
        {
            async Task Action() => await _sutUserService.SaveUserAsync(userName, "213213");

            await Assert.ThrowsAsync<ArgumentNullException>(Action);
        }

        [Theory]
        [InlineData(null)]
        public async Task SaveUserAsync_WhenPasswordIsMissing_ShouldThrowAnException(string password)
        {
            async Task Test() => await _sutUserService.SaveUserAsync("deded", password);

            await Assert.ThrowsAsync<ArgumentNullException>(Test);
        }

        //[Fact]
        //public async Task SaveUserAsync_WhenUserAlreadyExists_ShouldThrowDuplicateNameException()
        //{
        //    _mockUserRepository.Setup(x=>x.Get()).Returns();

        //    async Task Test() => await _sutUserService.SaveUserAsync("asd", "pass");

        //    await Assert.ThrowsAsync<DuplicateNameException>(Test);
        //}
    }
}
