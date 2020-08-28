//using Moq;
//using Xunit;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using System.Security.Authentication;

//namespace LetsDoIt.Moody.Application.UnitTests
//{
//    using Domain;
//    using Application.User;
//    using Persistance.Repositories.Base;
//    using LetsDoIt.Moody.Application.Utils;
//    using MockQueryable.Moq;

//    public class UserServiceTests
//    {
//        private readonly UserService _testing;

//        private readonly Mock<IEntityRepository<UserToken>> _mockUserTokenRepository;
//        private readonly Mock<IEntityRepository<User>> _mockUserRepository;
//        private readonly string _applicationKey = "something";
//        private readonly int _tokenExpirationMinutes = 123;

//        public UserServiceTests()
//        {
//            _mockUserRepository = new Mock<IEntityRepository<User>>();
//            _mockUserTokenRepository = new Mock<IEntityRepository<UserToken>>();

//            _testing = new UserService
//                (_mockUserRepository.Object,
//                _mockUserTokenRepository.Object,
//                _applicationKey,
//                _tokenExpirationMinutes);
//        }

//        [Fact]
//        public async Task AuthenticateAsync_UserExistsAndTokenIsNotNull_ReturnToken()
//        {
//            //Arrange
//            var username = "good.username";
//            var password = "good.password";

//            var token = new UserToken
//            {
//                Token = "good.token"
//            };

//            var users = new List<User>
//            {
//                new User()
//                {
//                    UserName = username,
//                    Password = ProtectionHelper.EncryptValue(password),
//                    UserToken = token
//                }
//            };

//            _mockUserRepository
//                .Setup(repo => repo.Get())
//                .Returns(users.AsQueryable().BuildMockDbSet().Object);

//            //Act
//            var actual = await _testing.AuthenticateAsync(username, password);

//            //Assert
//            Assert.NotNull(actual);
//            Assert.Equal(username, actual.Username);
//            Assert.Equal(token.Token, actual.Token);
//            Assert.True(DateTime.Now < actual.ExpirationDate);

//            //async Task Test() => await _testing.AuthenticateAsync(username, userpassword);
//            //var actual = await _testing.AuthenticateAsync(username, userpassword);
//            //Assert.Equal(token.Token, actual.Token);

//        }

//        [Fact]
//        public async Task AuthenticateAsync_UserDoesNotExistsInTheDatabase_ThrowAuthenticationException()
//        {
//            var username = "Test";
//            var userpassword = "12345";

//            async Task Test() => await _testing.AuthenticateAsync(username, userpassword);

//            await Assert.ThrowsAsync<AuthenticationException>(Test);
//        }

//        [Fact]
//        public async Task AuthenticateAsync_UserExistsWithoutToken_ShouldGenerateAToken()
//        {
//            var username = "Test";
//            var userpassword = "12345";

//            var userTokens = new UserToken
//            {
//                Token = null
//            };

//            var user = new List<User>
//            {
//                new User()
//                {
//                    Id = 1,
//                    UserName = username,
//                    Password = userpassword,
//                    UserToken = userTokens
//                }

//            };

//            _mockUserRepository.Setup(user => user.Get()).Returns(user.AsQueryable());

//            async Task Test() => await _testing.AuthenticateAsync(username, userpassword);

//            /*_mockUserTokenRepository.Verify(token =>
//                    token.UpdateAsync(It.Is<UserTokenEntity>(x => x.Token == )),
//                    Times.Once);*/
//        }

//        [Fact]
//        public async Task AuthenticateAsync_UserExistsWithExpiredToken_ShouldGenerateNewToken()
//        {
//            var username = "Test";
//            var userpassword = "12345";

//            var userTokens = new UserToken
//            {
//                Token = "jhafusfcuyfeuycewhvbcbhec",
//                ExpirationDate = DateTime.Now
//            };

//            var user = new List<User>
//            {
//                new User()
//                {
//                    UserName = username,
//                    Password = userpassword,
//                    UserToken = userTokens
//                }

//            };

//            _mockUserRepository.Setup(repo => repo.Get()).Returns(user.AsQueryable());

//            async Task Test() => await _testing.AuthenticateAsync(username, userpassword);

//            _mockUserTokenRepository.Verify(token =>
//                    token.UpdateAsync(It.Is<UserToken>(x => x.Token == userTokens.Token)),
//                    Times.Once);
//        }


//        [Fact]
//        public async Task SaveUserAsync_GivenNoException_ShouldInvokeUserRepositoryAddAsync()
//        {
//            var user = new User
//            {
//                UserName = "asd",
//                Password = "pass"
//            };

//            await _testing.SaveUserAsync(user.UserName, user.Password);

//            _mockUserRepository.Verify(ur =>
//                    ur.AddAsync(It.Is<User>(x => x.UserName == user.UserName)),
//                Times.Once);
//        }

//    }
//}