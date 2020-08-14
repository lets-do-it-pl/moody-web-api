using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.User;
using LetsDoIt.Moody.Persistance;
using LetsDoIt.Moody.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using MoqExpression;
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


        //NGuard does not throw exception
        [Theory]
        [InlineData(null)]
        public async Task SaveUserAsync_WhenUserNameIsMissing_ShouldThrowAnException(string userName)
        {
            async Task Action() => await _sutUserService.SaveUserAsync(userName, "213213");

            await Assert.ThrowsAsync<ArgumentNullException>(Action);
        }

        //NGuard does not throw exception
        [Theory]
        [InlineData(null)]
        public async Task SaveUserAsync_WhenPasswordIsMissing_ShouldThrowAnException(string password)
        {
            async Task Test() => await _sutUserService.SaveUserAsync("deded", password);

            await Assert.ThrowsAsync<ArgumentNullException>(Test);
        }


        //Gives ::::::  System.NotSupportedException : Unsupported expression: ... => ....Any<User>()
        //Extension methods(here: Queryable.Any) may not be used in setup / verification expressions.
        [Fact]
        public async Task SaveUserAsync_WhenUserAlreadyExists_ShouldThrowDuplicateNameException()
        {
            var user = new User()
            {
                UserName = "asd",
                Password = "pass"
            };


            //Can't mock static extension methods like .Any()!!!
            _mockUserRepository.Setup(x=>x.Get());

            async Task Test() => await _sutUserService.SaveUserAsync("asd", "pass");

            await Assert.ThrowsAsync<DuplicateNameException>(Test);
        }


        [Fact]
        public async Task SaveUserAsync_GivenNoException_ShouldInvokeUserRepositoryAddAsync()
        {
            await _sutUserService.SaveUserAsync("asd", "pass");

           _mockUserRepository.Verify(ur=>ur.AddAsync(It.IsAny<User>()),
               Times.Once);
        }
    }
}
