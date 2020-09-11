﻿using System.Net.Http;
using System.Threading.Tasks;
using LetsDoIt.Moody.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace LetsDoIt.Moody.Application.IntegrationTests
{
    public class HealthCheckTests:IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;

        public HealthCheckTests(WebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task HealthCheck_ReturnsOk()
        {
            var response = await _httpClient.GetAsync("/healthcheck");

            response.EnsureSuccessStatusCode();

            // Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        }
    }
}
