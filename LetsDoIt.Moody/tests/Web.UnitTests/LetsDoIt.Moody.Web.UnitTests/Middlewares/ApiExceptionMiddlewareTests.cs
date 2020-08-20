using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LetsDoIt.Moody.Web.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace LetsDoIt.Moody.Web.UnitTests.Middlewares
{
    public class ApiExceptionMiddlewareTests
    {
        [Fact]
        public async Task WhenAnUnExpectedExceptionIsRaised_CustomExceptionMiddlewareShouldHandleItToCustomErrorResponseAndInternalServerErrorHttpStatus()
        {
            // Arrange
            ApiExceptionOptions mockOptions=new ApiExceptionOptions();
            ILogger<ApiExceptionMiddleware> mockLogger = new NullLogger<ApiExceptionMiddleware>();

            var middleware = new ApiExceptionMiddleware(mockOptions, next: (innerHttpContext) => 
                throw new Exception("Test exception"),mockLogger);

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = await reader.ReadToEndAsync();
            var objResponse = JsonConvert.DeserializeObject<ApiError>(streamText);

            //Assert
            objResponse
                .Should()
                .BeEquivalentTo(new ApiError() {   = "Unexpected error", Description = "Unexpected error" });

            context.Response.StatusCode
                .Should()
                .Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
