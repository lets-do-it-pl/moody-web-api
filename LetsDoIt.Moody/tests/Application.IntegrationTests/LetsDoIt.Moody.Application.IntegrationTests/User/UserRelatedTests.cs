using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Xunit;

namespace LetsDoIt.Moody.Application.IntegrationTests.User
{
    using Web.Entities.Requests;
    using Web;

    public class UserRelatedTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly Uri _baseUri;

        public UserRelatedTests(CustomWebApplicationFactory<Startup> factory)
        {
            _baseUri = new Uri("http://localhost/api/users");
            _factory = factory;
            _client = factory.CreateDefaultClient();
            _factory.ResetDbForTests();

            var token = factory.GenerateTempSaveUserTokenForTests();
            _client.DefaultRequestHeaders.Add("Authorization",token); 
        }

        private StringContent GetStringContent(string token, SaveUserRequest saveUserRequest)
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(saveUserRequest));

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            return httpContent;
        }

        [Fact]
        public async Task SaveUser_ShouldReturnCreatedStatusCodeAndRecordToDatabase()
        {
            // Arrange
            var token = _factory.GenerateTempSaveUserTokenForTests();

            var saveUserRequest = new SaveUserRequest
            {
                Username = "good.username",
                Password = "good.password"
            };

            var httpContent = GetStringContent(token, saveUserRequest);

            // Act
            var response = await _client.PostAsync("/api/users", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var user = await _factory.UserRepositoryVar.GetAsync(u => u.UserName == "good.username");

            Assert.NotNull(user);
            Assert.Equal("good.username", user.UserName);

        }
                
        [Fact]
        public async Task SaveUser_WhenUserAlreadyExistsShouldReturnBadRequest()
        {

            var token = _factory.GenerateTempSaveUserTokenForTests();

            var saveUserRequest = new SaveUserRequest
            {
                Username = "good.username",
                Password = "good.password"
            };

            var httpContent = GetStringContent(token, saveUserRequest);


            // Act
            var response1 = await _client.PostAsync("/api/users", httpContent);
            var response2 = await _client.PostAsync("/api/users", httpContent);

            response1.StatusCode.Should().Be(HttpStatusCode.Created);
            response2.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }

        [Fact]
        public async Task Authenticate_WhenUsernameDoesNotExists_ShouldReturnBadRequest()
        {

            var parametersToAdd = new Dictionary<string, string> { { "username", "asd" }, { "password", "sss" } };
            var newUri = QueryHelpers.AddQueryString(_baseUri.OriginalString, parametersToAdd);

            var httpContent = new StringContent("");
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _client.PostAsync(newUri, httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Authenticate_ShouldCheckDatabaseAndReturnOk()
        {
            //Save User to 
            var token = _factory.GenerateTempSaveUserTokenForTests();

            var saveUserRequest = new SaveUserRequest
            {
                Username = "good.username",
                Password = "good.password"
            };

            var httpContentSaveUser = GetStringContent(token, saveUserRequest);

            var responseSaveUser = await _client.PostAsync("/api/users", httpContentSaveUser);

            responseSaveUser.StatusCode.Should().Be(HttpStatusCode.Created);

            //Authenticate
            var parametersToAdd = new Dictionary<string, string> { { "username", "good.username" }, { "password", "good.password" } };
            var newUri = QueryHelpers.AddQueryString(_baseUri.OriginalString + "/authenticate", parametersToAdd);

            var httpContentAuthenticate = new StringContent("");
            httpContentAuthenticate.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var responseAuthenticate = await _client.PostAsync(newUri, httpContentAuthenticate);

            responseAuthenticate.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        [Fact]
        public async Task Authenticate_WhenPasswordIsWrong_ShouldReturnBadRequest()
        {
            //Save User to Database
            var token = _factory.GenerateTempSaveUserTokenForTests();
            var saveUserRequest = new SaveUserRequest
            {
                Username = "good.username",
                Password = "good.password"
            };

            var httpContentSaveUser = GetStringContent(token, saveUserRequest);

            var responseSaveUser = await _client.PostAsync("/api/users", httpContentSaveUser);

            responseSaveUser.StatusCode.Should().Be(HttpStatusCode.Created);

            //Authenticate
            var parametersToAdd = new Dictionary<string, string> { { "username", "good.username" }, { "password", "bad.password" } };
            var newUri = QueryHelpers.AddQueryString(_baseUri.OriginalString, parametersToAdd);

            var httpContent = new StringContent("");
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _client.PostAsync(newUri, httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }
    }
}