//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;
//using FluentAssertions;
//using Microsoft.AspNetCore.WebUtilities;
//using Newtonsoft.Json;
//using Xunit;

//namespace LetsDoIt.Moody.Application.IntegrationTests.Category
//{
//    using Web;
//    using Web.Entities.Requests;
//    using IntegrationTests;

//    public class CategoryRelatedTests : IClassFixture<CustomWebApplicationFactory<Startup>>
//    {
//        private readonly HttpClient _client;
//        private readonly CustomWebApplicationFactory<Startup> _factory;
//        private readonly Uri _baseUri;

//        public CategoryRelatedTests(CustomWebApplicationFactory<Startup> factory)
//        {

//            _baseUri = new Uri("http://localhost/api/users");
//            _factory = factory;
//            _client = factory.CreateDefaultClient();
//            _factory.ResetDbForTests();
//            var userToken = _factory.GetUserTokenForTestsAndRecordToDatabase();

//            _client.DefaultRequestHeaders.Add("Authorization", userToken.Token);
//        }

//        //[Fact]
//        //public async Task Insert_ShouldReturnCreatedStatusCodeAndRecordToDatabase()
//        //{
//        //    // Arrange
//        //    var categoryInsertRequest = new CategoryInsertRequest
//        //    {
//        //        Order = 1,
//        //        Image = "USrCELxGejBZI4W/Llsvmw==\r\n",
//        //        Name = "good.category"
//        //    };

//        //    var httpContent = new StringContent(JsonConvert.SerializeObject(categoryInsertRequest));
//        //    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//        //    // Act
//        //    var response = await _client.PostAsync("/api/categories", httpContent);

//        //    // Assert
//        //    response.StatusCode.Should().Be(HttpStatusCode.OK);
//        //}

//        [Fact]
//        public async Task SaveUser_WhenUserAlreadyExistsShouldReturnBadRequest()
//        {

//            var saveUserRequest = new SaveClientRequest
//            {
//                Username = "good.username",
//                Password = "good.password"
//            };

//            var httpContent = new StringContent(JsonConvert.SerializeObject(saveUserRequest));
//            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


//            // Act
//            var response1 = await _client.PostAsync("/api/users", httpContent);
//            var response2 = await _client.PostAsync("/api/users", httpContent);

//            response1.StatusCode.Should().Be(HttpStatusCode.Created);
//            response2.StatusCode.Should().Be(HttpStatusCode.BadRequest);

//        }

//        [Fact]
//        public async Task Authenticate_WhenUsernameDoesNotExists_ShouldReturnBadRequest()
//        {

//            var parametersToAdd = new Dictionary<string, string> { { "username", "asd" }, { "password", "sss" } };
//            var newUri = QueryHelpers.AddQueryString(_baseUri.OriginalString, parametersToAdd);

//            var httpContent = new StringContent("");
//            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//            var response = await _client.PostAsync(newUri, httpContent);

//            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task Authenticate_ShouldCheckDatabaseAndReturnOk()
//        {
//            var saveUserRequest = new SaveClientRequest
//            {
//                Username = "good.username",
//                Password = "good.password"
//            };

//            var httpContentSaveUser = new StringContent(JsonConvert.SerializeObject(saveUserRequest));
//            httpContentSaveUser.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//            var responseSaveUser = await _client.PostAsync("/api/users", httpContentSaveUser);

//            responseSaveUser.StatusCode.Should().Be(HttpStatusCode.Created);

//            //Authenticate
//            var parametersToAdd = new Dictionary<string, string> { { "username", "good.username" }, { "password", "good.password" } };
//            var newUri = QueryHelpers.AddQueryString(_baseUri.OriginalString + "/authenticate", parametersToAdd);

//            var httpContentAuthenticate = new StringContent("");
//            httpContentAuthenticate.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//            var responseAuthenticate = await _client.PostAsync(newUri, httpContentAuthenticate);

//            responseAuthenticate.StatusCode.Should().Be(HttpStatusCode.OK);

//        }

//        [Fact]
//        public async Task Authenticate_WhenPasswordIsWrong_ShouldReturnBadRequest()
//        {
//            //Save Client to Database
//            var saveUserRequest = new SaveClientRequest
//            {
//                Username = "good.username",
//                Password = "good.password"
//            };

//            var httpContentSaveUser = new StringContent(JsonConvert.SerializeObject(saveUserRequest));
//            httpContentSaveUser.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//            var responseSaveUser = await _client.PostAsync("/api/users", httpContentSaveUser);

//            responseSaveUser.StatusCode.Should().Be(HttpStatusCode.Created);

//            //Authenticate
//            var parametersToAdd = new Dictionary<string, string> { { "username", "good.username" }, { "password", "bad.password" } };
//            var newUri = QueryHelpers.AddQueryString(_baseUri.OriginalString, parametersToAdd);

//            var httpContent = new StringContent("");
//            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//            var response = await _client.PostAsync(newUri, httpContent);

//            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

//        }
//    }
//}