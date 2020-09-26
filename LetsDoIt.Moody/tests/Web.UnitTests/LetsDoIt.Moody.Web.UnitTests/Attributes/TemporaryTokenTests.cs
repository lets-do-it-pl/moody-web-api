using Moq;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace LetsDoIt.Moody.Web.UnitTests.Attributes
{
    using Web;
    using Infrastructure;

    public class TemporaryTokenTests
    {
        private const string TokenHeaderName = "Token";
        private readonly AuthorizationByTempToken _testing;
        private readonly ActionExecutedContext _context;
        private readonly ActionExecutingContext _actionExecutingContext;

        public TemporaryTokenTests()
        {
            _testing = new AuthorizationByTempToken();

            var httpContext = new DefaultHttpContext();

            var actionContext = new ActionContext
            (
                httpContext,
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>()
            );

            var metadata = new List<IFilterMetadata>();

            _actionExecutingContext = new ActionExecutingContext
                (
                    actionContext,
                    metadata,
                    new Dictionary<string, object>(),
                    Mock.Of<Controller>()
                );

            _context = new ActionExecutedContext(actionContext, metadata, Mock.Of<Controller>());
        }

        [Fact]
        public async Task OnActionExecutionAsync_SaveUserRequestWithoutGuid_ShouldReturnUnauthorizedResult()
        {
            Task<ActionExecutedContext> Next() => Task.FromResult(_context);

            await _testing.OnActionExecutionAsync(_actionExecutingContext, Next);

            _actionExecutingContext.Result.Should().BeOfType<UnauthorizedResult>();

        }

        [Theory]
        [InlineData("bad.guid")]
        [InlineData(null)]
        public async Task OnActionExecutionAsync_WhenTokenIsNotValid_ShouldReturnUnauthorizedResult(string token)
        {
            _actionExecutingContext.HttpContext.Request.Headers.Add(TokenHeaderName, token);

            ActionExecutionDelegate Next = () => {
                var ctx = _context;
                return Task.FromResult(ctx);
            };

            await _testing.OnActionExecutionAsync(_actionExecutingContext, Next);

            _actionExecutingContext.Result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldInvokeUserServiceIsTokenValid()
        {
            var token = TemporaryToken.GenerateTemporaryToken();

            _actionExecutingContext.HttpContext.Request.Headers.Add(TokenHeaderName, token);

            Task<ActionExecutedContext> Next() => Task.FromResult(_context);

            await _testing.OnActionExecutionAsync(_actionExecutingContext, Next);
        }
    }
}
