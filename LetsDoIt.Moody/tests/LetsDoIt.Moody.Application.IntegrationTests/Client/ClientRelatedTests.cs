//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;
//using FluentAssertions;
//using LetsDoIt.Moody.Web;
//using LetsDoIt.Moody.Web.Entities.Requests;
//using Microsoft.AspNetCore.WebUtilities;
//using Newtonsoft.Json;
//using Xunit;

//namespace LetsDoIt.Moody.Application.IntegrationTests.Client
//{
//    public class ClientRelatedTests : IClassFixture<CustomWebApplicationFactory<Startup>>
//    {
//        private readonly HttpClient _client;
//        private readonly CustomWebApplicationFactory<Startup> _factory;
//        private readonly Uri _baseUri;

//        public ClientRelatedTests(CustomWebApplicationFactory<Startup> factory)
//        {
//            _baseUri = new Uri("http://localhost/api/clients");
//            _factory = factory;
//            _client = factory.CreateDefaultClient();
//            _factory.ResetDbForTests();

//            var token = factory.GenerateTempSaveUserTokenForTests();
//            _client.DefaultRequestHeaders.Add("Authorization", token);
//        }

//        private StringContent GetStringContent(SaveClientRequest saveClientRequest)
//        {
//            var httpContent = new StringContent(JsonConvert.SerializeObject(saveClientRequest));

//            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//            return httpContent;
//        }

//        [Fact]
//        public async Task SaveClient_ShouldReturnCreatedStatusCodeAndRecordToDatabase()
//        {
//            // Arrange
//            var createDate = DateTime.UtcNow;
//            var saveUserRequest = new SaveClientRequest
//            {
//                Username = "good.username",
//                Password = "good.password"
//            };

//            var httpContent = GetStringContent(saveUserRequest);

//            // Act
//            var response = await _client.PostAsync("/api/clients", httpContent);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.Created);

//            var user = await _factory.UserRepositoryVar.GetAsync(u => u.UserName == "good.username");

//            Assert.NotNull(user);
//            Assert.True(createDate < user.CreateDate);
//        }

//        [Fact]
//        public async Task SaveUserClient_WhenClientAlreadyExistsShouldReturnBadRequest()
//        {
//            var saveUserRequest = new SaveClientRequest
//            {
//                Username = "good.username",
//                Password = "good.password"
//            };

//            var httpContent = GetStringContent(saveUserRequest);

//            // Act
//            var response1 = await _client.PostAsync("/api/clients", httpContent);
//            var response2 = await _client.PostAsync("/api/clients", httpContent);

//            response1.StatusCode.Should().Be(HttpStatusCode.Created);
//            response2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task Authenticate_WhenUsernameDoesNotExists_ShouldReturnBadRequest()
//        {
//            var (newUri, httpContent) = GetUriAndContent("notExist.UserName", "notExist.Password");

//            var response = await _client.PostAsync(newUri, httpContent);

//            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task Authenticate_ShouldCheckDatabaseAndReturnOk()
//        {
//            //Save User to 

//            var goodUsername = "good.username";
//            var goodPassword = "good.password";
//            var saveUserRequest = new SaveClientRequest
//            {
//                Username = goodUsername,
//                Password = goodPassword
//            };

//            var httpContentSaveUser = GetStringContent(saveUserRequest);

//            var responseSaveUser = await _client.PostAsync("/api/clients", httpContentSaveUser);

//            responseSaveUser.StatusCode.Should().Be(HttpStatusCode.Created);

//            //Authenticate
//            var (newUri, httpContent) = GetUriAndContent(goodUsername, goodPassword, "/authenticate");

//            var responseAuthenticate = await _client.PostAsync(newUri, httpContent);

//            responseAuthenticate.StatusCode.Should().Be(HttpStatusCode.OK);
//        }

//        [Fact]
//        public async Task Authenticate_WhenPasswordIsWrong_ShouldReturnBadRequest()
//        {
//            //Save User to Database
//            var goodUsername = "good.username";

//            var saveUserRequest = new SaveClientRequest
//            {
//                Username = goodUsername,
//                Password = "good.password"
//            };

//            var httpContentSaveUser = GetStringContent(saveUserRequest);

//            var responseSaveUser = await _client.PostAsync("/api/clients", httpContentSaveUser);

//            responseSaveUser.StatusCode.Should().Be(HttpStatusCode.Created);

//            //Authenticate
//            var (newUri, httpContent) = GetUriAndContent(goodUsername, "bad.password");

//            var response = await _client.PostAsync(newUri, httpContent);

//            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

//        }

//        private (string, StringContent) GetUriAndContent(string username, string password, string queryString = "")
//        {
//            var parametersToAdd = new Dictionary<string, string>
//            {
//                {"username", username},
//                {"password", password}
//            };

//            var newUri = QueryHelpers.AddQueryString(_baseUri.OriginalString + queryString, parametersToAdd);

//            var httpContent = new StringContent("");
//            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//            return (newUri, httpContent);
//        }
//    }
//}