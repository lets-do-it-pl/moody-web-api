using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using LetsDoIt.Moody.Web.Middleware;
using Microsoft.AspNetCore.Http;
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
        public async Task WhenAnUnExpectedExceptionIsRaised_CustomExceptionMiddlewareShouldHandleItToApiErrorResponseAndReturnInternalServerErrorHttpStatusCode()
        {
            // Arrange
            ILogger<ApiExceptionMiddleware> mockLogger = new NullLogger<ApiExceptionMiddleware>();

            var middleware = new ApiExceptionMiddleware( next: (innerHttpContext) => 
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

            //objResponse
            //    .Should()
            //    .BeEquivalentTo(new ApiError() {  Title = "Some kind of error occurred in the API. Please use the id and contact our " +
            //                                                                                                 "support team if the problem persists."});

            context.Response.StatusCode
                .Should()
                .Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
