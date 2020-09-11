using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using LetsDoIt.Moody.Web;
using LetsDoIt.Moody.Web.Entities.Requests;
using Newtonsoft.Json;
using Xunit;

namespace LetsDoIt.Moody.Application.IntegrationTests.User
{
    public class UserRelatedTests:IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public UserRelatedTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateDefaultClient(new Uri("http://localhost/api/users"));
        }

        [Fact]
        public async Task SaveUser_ShouldReturnCreatedStatusCodeAndRecordToDatabase()
        {
            // Arrange
            var saveUserRequest = new SaveUserRequest
            {
                Username = "good.username",
                Password = "good.password"
            };

            var httpContent = new StringContent(JsonConvert.SerializeObject(saveUserRequest));
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await _client.PostAsync("/api/users", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var user = await _factory.UserRepositoryVar.GetAsync(u => u.UserName == "good.username");

            Assert.NotNull(user);
            Assert.Equal("good.username", user.UserName);

            _factory.ResetDbForTests();
        }

        [Fact]
        public async Task SaveUser_WhenUserAlreadyExistsShouldReturnBadRequest()
        {
            var saveUserRequest = new SaveUserRequest
            {
                Username = "good.username",
                Password = "good.password"
            };

            var httpContent = new StringContent(JsonConvert.SerializeObject(saveUserRequest));
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response1 = await _client.PostAsync("/api/users", httpContent);
            var response2 = await _client.PostAsync("/api/users", httpContent);

            response1.StatusCode.Should().Be(HttpStatusCode.Created);
            response2.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            _factory.ResetDbForTests();
        }

        //[Fact]
        //public async Task Authenticate_WhenUsernameOrPasswordIsWrong_ShouldReturnBadRequest()
        //{

        //    var httpContent = new StringContent(JsonConvert.SerializeObject(new { username = "asd", password = "sss" }));
        //    var response1 = await _client.PostAsync("/api/users/authenticate", httpContent);

        //    response1.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        //}
    }
}
