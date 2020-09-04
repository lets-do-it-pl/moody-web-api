using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LetsDoIt.Moody.Web.Entities.Requests;
using Newtonsoft.Json;
using Xunit;

namespace LetsDoIt.Moody.Application.IntegrationTests.User
{
    public class UserOperationsTests:IntegrationTestBase
    {
        [Fact]
        public async Task SaveUser_GivenNoException_ShouldReturnCreated()
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
            var response = await TestClient.PostAsync("/api/users",httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
