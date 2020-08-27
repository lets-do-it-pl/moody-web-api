using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using LetsDoIt.Moody.Application.User;

namespace LetsDoIt.Moody.Application.UnitTests.User
{
    using Domain;
    using Persistance.Repositories.Base;

    public class UserSeviceTests
    {
        private readonly Mock<IEntityRepository<UserToken>> _mockUserTokenRepository;
        private readonly Mock<IEntityRepository<User>> _mockUserRepository;
        private readonly IUserService _testing;
        private readonly string _applicationKey = "something";
        private readonly int _tokenExpirationMinutes = 123;

        public UserSeviceTests()
        {
            _mockUserRepository = new Mock<IEntityRepository<User>>();
            _mockUserTokenRepository=new Mock<IEntityRepository<UserToken>>();
            _testing=new UserService(_mockUserRepository.Object,_mockUserTokenRepository.Object, _applicationKey, _tokenExpirationMinutes);
        }

        [Fact]
        public async Task SaveUserAsync_WhenUserNameIsNull_ShouldThrowArgumentNullException()
        {
            string userName = null;
            string password = "pass";

            async Task Test() => await _testing.SaveUserAsync(userName, password);

            await Assert.ThrowsAsync<ArgumentNullException>(Test);
        }

        [Fact]
        public async Task SaveUserAsync_WhenPasswordIsNull_ShouldThrowArgumentNullException()
        {
            string userName = "asd";
            string password = null;

            async Task Test() => await _testing.SaveUserAsync(userName, password);

            await Assert.ThrowsAsync<ArgumentNullException>(Test);
        }

        [Theory]
        [InlineData("","pass")]
        [InlineData(" ","pass")]
        [InlineData("asd","")]
        [InlineData("asd"," ")]
        public async Task SaveUserAsync_WhenPasswordOrUserNameEmptyOrWhiteSpace_ShouldThrowArgumentException(string userName,string password)
        {
            async Task Test() => await _testing.SaveUserAsync(userName, password);

            await Assert.ThrowsAsync<ArgumentException>(Test);
        }

        [Fact]
        public async Task SaveUserAsync_WhenUserAlreadyExists_ShouldThrowDuplicateNameException()
        {
            var username = "asd";
            var password = "dsa";

            var userList = new List<User>
            {
                new User() { UserName = "asd"}

            }.AsQueryable();

            _mockUserRepository.Setup(repo=>repo.Get()).Returns(userList);

            async Task Test() => await _testing.SaveUserAsync(username, password);

            await Assert.ThrowsAsync<DuplicateNameException>(Test);
        }

        [Fact]
        public async Task SaveUserAsync_GivenNoException_ShouldInvokeUserRepositoryAddAsync()
        {
            var user = new User
            {
                UserName = "asd",
                Password = "pass"
            };

            await _testing.SaveUserAsync(user.UserName,user.Password);

           _mockUserRepository.Verify(ur=>
                   ur.AddAsync(It.Is<User>(x => x.UserName == user.UserName)),
               Times.Once);
        }
    }
}
