﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using LetsDoIt.Moody.Persistance;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests
{
    using Domain;
    using Persistance.Repositories.Base;
    using User;


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

        [Fact]
        public async Task SaveUserAsync_WhenUserAlreadyExists_ShouldThrowDuplicateNameException()
        {
            var data = new List<User>
            {
                new User() { UserName = "asd"}

            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);
            _mockUserRepository.Setup(x=>x.Get()).Returns(mockContext.Object.Users);

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
