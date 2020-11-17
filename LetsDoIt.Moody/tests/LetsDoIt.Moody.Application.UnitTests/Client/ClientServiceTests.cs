//using Moq;
//using Xunit;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using System.Data;
//using System.Security.Authentication;
//using LetsDoIt.Moody.Application.Client;
//using LetsDoIt.Moody.Application.Security;
//using MockQueryable.Moq;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;


//namespace LetsDoIt.Moody.Application.UnitTests.Client
//{
//    using Domain;
//    using Persistance.Repositories.Base;
//    using Infrastructure.Utils;

//    public class ClientServiceTests
//    {
//        //private readonly IClientService _testing;
//        //private readonly Mock<IEntityRepository<Client>> _mockUserRepository;
//        //private readonly Mock<ISecurityService> _securityServiceMock;
//        //private readonly string _applicationKey = "d1442e0f-01e0-4074-bdae-28b8f57a6b40";
//        //private readonly int _tokenExpirationMinutes = 123;

//        //public ClientServiceTests()
//        //{
//        //    _mockUserRepository = new Mock<IEntityRepository<Client>>();
//        //    _securityServiceMock = new Mock<ISecurityService>();

//        //    _testing = new ClientService
//        //        (_mockUserRepository.Object,
//        //        _securityServiceMock.Object,
//        //        new Mock<ILogger<ClientService>>().Object);
//        //}

//        //[Fact]
//        //public async Task AuthenticateAsync_UserExistsAndTokenIsNotNull_ReturnToken()
//        //{
//        //    //Arrange
//        //    var username = "good.username";
//        //    var password = "good.password";

//        //    var token = new ClientToken
//        //    {
//        //        Token = "good.token",
//        //        ExpirationDate = DateTime.UtcNow.AddMinutes(12)
//        //    };

//        //    var users = new List<Client>
//        //    {
//        //        new Client()
//        //        {
//        //            UserName = username,
//        //            Password = ProtectionHelper.EncryptValue(username + password),
//        //            ClientToken = token
//        //        }
//        //    };

//        //    //Act
//        //    _mockUserRepository.Setup(repo => repo.Get()).
//        //        Returns(users.AsQueryable().BuildMockDbSet().Object);

//        //    var actual = await _testing.AuthenticateAsync(username, password);

//        //    //Assert
//        //    Assert.NotNull(actual);
//        //    Assert.Equal(username, actual.Username);
//        //    Assert.Equal(token.Token, actual.Token);
//        //    Assert.True(DateTime.UtcNow < actual.ExpirationDate);
//        //}

//        //[Fact]
//        //public async Task AuthenticateAsync_UserDoesNotExistsInTheDatabase_ThrowAuthenticationException()
//        //{
//        //    var username = "bad.username";
//        //    var password = "bad.password";

//        //    _mockUserRepository.Setup(repo => repo.Get()).Throws(new AuthenticationException());

//        //    async Task Test() => await _testing.AuthenticateAsync(username, password);

//        //    await Assert.ThrowsAsync<AuthenticationException>(Test);
//        //}

//        //[Fact]
//        //public async Task AuthenticateAsync_UserExistsWithoutToken_ShouldGenerateAToken()
//        //{
//        //    var username = "good.username";
//        //    var password = "good.password";

//        //    var users = new List<Client>
//        //    {
//        //        new Client()
//        //        {
//        //            Id = 1,
//        //            UserName = username,
//        //            Password = ProtectionHelper.EncryptValue(username + password),
//        //            ClientToken = null
//        //        }
//        //    };

//        //    var userToken = new ClientToken
//        //    {
//        //        UserId = 1,
//        //        Token = "good.token",
//        //        ExpirationDate = DateTime.UtcNow.AddMinutes(5)
//        //    };

//        //    _mockUserTokenRepository
//        //        .Setup(repo => repo.UpdateAsync(It.IsAny<ClientToken>()))
//        //        .ReturnsAsync(userToken);

//        //    _mockUserRepository.Setup(user => user.Get()).
//        //        Returns(users.AsQueryable().BuildMockDbSet().Object);

//        //    var actual = await _testing.AuthenticateAsync(username, password);

//        //    _mockUserTokenRepository.Verify(token =>
//        //            token.UpdateAsync(It.IsAny<ClientToken>()),
//        //            Times.Once);
//        //}

//        //[Fact]
//        //public async Task AuthenticateAsync_UserExistsWithExpiredToken_ShouldGenerateNewToken()
//        //{
//        //    var username = "good.username";
//        //    var password = "good.password";

//        //    var expiredUserToken = new ClientToken
//        //    {
//        //        UserId = 1,
//        //        Token = "expired.token",
//        //        ExpirationDate = DateTime.UtcNow
//        //    };

//        //    var userToken = new ClientToken
//        //    {
//        //        UserId = 1,
//        //        Token = "good.token",
//        //        ExpirationDate = DateTime.UtcNow.AddMinutes(5)
//        //    };

//        //    var users = new List<Client>
//        //    {
//        //        new Client()
//        //        {
//        //            Id = 1,
//        //            UserName = username,
//        //            Password =  ProtectionHelper.EncryptValue(username + password),
//        //            ClientToken = expiredUserToken
//        //        }

//        //    };

//        //    _mockUserTokenRepository
//        //        .Setup(repo => repo.UpdateAsync(It.IsAny<ClientToken>()))
//        //        .ReturnsAsync(userToken);

//        //    _mockUserRepository.Setup(repo => repo.Get()).
//        //        Returns(users.AsQueryable().BuildMockDbSet().Object);

//        //    var actual = await _testing.AuthenticateAsync(username, password);

//        //    Assert.NotEqual(expiredUserToken.Token, actual.Token);
//        //    Assert.Equal(userToken.Token, actual.Token);

//        //    _mockUserTokenRepository.Verify(token =>
//        //            token.UpdateAsync(It.IsAny<ClientToken>()),
//        //            Times.Once);
//        //}

//        //[Fact]
//        //public async Task AuthenticateAsync_UserNameIsNull_ThrowsArgumentNullException()
//        //{
//        //    string username = null;
//        //    string password = "Test";

//        //    async Task Test() => await _testing.AuthenticateAsync(username, password);

//        //    await Assert.ThrowsAsync<ArgumentNullException>(Test);
//        //}


//        //[Fact]
//        //public async Task AuthenticateAsync_PasswordNameIsNull_ThrowsArgumentNullException()
//        //{
//        //    string username = "Test";
//        //    string password = null;

//        //    async Task Test() => await _testing.AuthenticateAsync(username, password);

//        //    await Assert.ThrowsAsync<ArgumentNullException>(Test);
//        //}

//        //[Fact]
//        //public async Task SaveUserAsync_WhenUserNameIsNull_ShouldThrowArgumentNullException()
//        //{
//        //    string userName = null;
//        //    string password = "pass";

//        //    async Task Test() => await _testing.SaveClientAsync(userName, password);

//        //    await Assert.ThrowsAsync<ArgumentNullException>(Test);
//        //}

//        //[Fact]
//        //public async Task SaveUserAsync_WhenPasswordIsNull_ShouldThrowArgumentNullException()
//        //{
//        //    string userName = "asd";
//        //    string password = null;

//        //    async Task Test() => await _testing.SaveClientAsync(userName, password);

//        //    await Assert.ThrowsAsync<ArgumentNullException>(Test);
//        //}

//        //[Theory]
//        //[InlineData("", "pass")]
//        //[InlineData(" ", "pass")]
//        //[InlineData("asd", "")]
//        //[InlineData("asd", " ")]
//        //public async Task SaveUserAsync_WhenPasswordOrUserNameEmptyOrWhiteSpace_ShouldThrowArgumentException(string userName, string password)
//        //{
//        //    async Task Test() => await _testing.SaveClientAsync(userName, password);

//        //    await Assert.ThrowsAsync<ArgumentException>(Test);
//        //}

//        //[Fact]
//        //public async Task SaveUserAsync_WhenUserAlreadyExists_ShouldThrowDuplicateNameException()
//        //{
//        //    var username = "asd";
//        //    var password = "dsa";

//        //    var userList = new List<Client>
//        //    {
//        //        new Client() { UserName = "asd"}

//        //    }.AsQueryable();

//        //    _mockUserRepository.Setup(repo => repo.Get()).Returns(userList);

//        //    async Task Test() => await _testing.SaveClientAsync(username, password);

//        //    await Assert.ThrowsAsync<DuplicateNameException>(Test);
//        //}

//        //[Fact]
//        //public async Task SaveUserAsync_GivenNoException_ShouldInvokeUserRepositoryAddAsync()
//        //{
//        //    var user = new Client
//        //    {
//        //        UserName = "asd",
//        //        Password = "pass"
//        //    };

//        //    await _testing.SaveClientAsync(user.UserName, user.Password);

//        //    _mockUserRepository.Verify(ur =>
//        //            ur.AddAsync(It.Is<Client>(x => x.UserName == user.UserName)),
//        //        Times.Once);
//        //}


//        //[Theory]
//        //[InlineData("")]
//        //[InlineData(" ")]
//        //[InlineData(null)]
//        //public async Task ValidateTokenAsync_WhenTokenIsEmptyOrWhitespaceOrNull_ShouldReturnFalse(string token)
//        //{
//        //    var result = await _testing.ValidateTokenAsync(token);

//        //    Assert.False(result);
//        //}

//        //[Fact]
//        //public async Task ValidateTokenAsync_WhenTokenIsWrong_ShouldReturnFalse()
//        //{
//        //    string token = "bad.token";

//        //    var user = new Client
//        //    {
//        //        Id = 3,
//        //        CreateDate = DateTime.UtcNow,
//        //        IsDeleted = false,
//        //        Password = "good.password",
//        //        UserName = "good.username",

//        //    };

//        //    var userToken = new ClientToken
//        //    {
//        //        UserId = 3,
//        //        Token = "good.token",
//        //        Client = user,
//        //        ExpirationDate = DateTime.UtcNow.AddMinutes(999999)
//        //    };

//        //    user.ClientToken = userToken;

//        //    var userTokens = new List<ClientToken>
//        //    {
//        //        userToken
//        //    };

//        //    _mockUserTokenRepository.Setup(repo => repo.Get()).Returns(userTokens.AsQueryable().BuildMockDbSet().Object);

//        //    var result = await _testing.ValidateTokenAsync(token);

//        //    Assert.False(result);
//        //}

//        //[Fact]
//        //public async Task ValidateTokenAsync_WhenUserIsDeleted_ShouldReturnFalse()
//        //{
//        //    string token = "good.token";

//        //    var user = new Client
//        //    {
//        //        Id = 3,
//        //        CreateDate = DateTime.UtcNow,
//        //        IsDeleted = true,
//        //        Password = "good.password",
//        //        UserName = "good.username",

//        //    };

//        //    var userToken = new ClientToken
//        //    {
//        //        UserId = 3,
//        //        Token = token,
//        //        Client = user,
//        //        ExpirationDate = DateTime.UtcNow.AddMinutes(999999)
//        //    };

//        //    user.ClientToken = userToken;

//        //    var userTokens = new List<ClientToken>
//        //    {
//        //        userToken
//        //    };

//        //    _mockUserTokenRepository.Setup(repo => repo.Get()).Returns(userTokens.AsQueryable().BuildMockDbSet().Object);

//        //    var result = await _testing.ValidateTokenAsync(token);

//        //    Assert.False(result);
//        //}
//    }
//}
