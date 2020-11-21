using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace LetsDoIt.Moody.Web.UnitTests.Middlewares
{
    using Middleware;

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

            //Act
            await middleware.Invoke(context);
          
            //Assert
            context.Response.StatusCode
                .Should()
                .Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
