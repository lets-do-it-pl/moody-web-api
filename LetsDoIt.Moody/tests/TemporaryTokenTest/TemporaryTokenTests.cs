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

namespace LetsDoIt.Moody.TemporaryToken.Test
{
    using Web;
    using Infrastructure;
    using LetsDoIt.Moody.Web.Controllers;
    using LetsDoIt.Moody.Application.User;

    public class TemporaryTokenTests
    {
        private readonly ActionExecutedContext _context;
        private readonly Mock<ITemporaryToken> _temporaryToken;
        private readonly TemporaryTokenValidatorAttribute _testing;
        private readonly ActionExecutingContext _actionExecutingContext;

        public TemporaryTokenTests()
        {
            _temporaryToken = new Mock<ITemporaryToken>();
            _testing = new TemporaryTokenValidatorAttribute();

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

        [Fact]
        public async Task OnActionExecutionAsync_WhenTokenIsNotValid_ShouldReturnUnauthorizedResult()
        {
            var guid = "bad.guid";

            _actionExecutingContext.HttpContext.Request.Headers.Add("Validate", guid);

            _temporaryToken.Setup(token => token.TemporaryTokenValidator(guid)).Returns(false);

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
            var guid = "good.guid";

            _actionExecutingContext.HttpContext.Request.Headers.Add("Validate", guid);

            _temporaryToken.Setup(token => token.TemporaryTokenValidator(guid)).Returns(true);

            Task<ActionExecutedContext> Next() => Task.FromResult(_context);

            await _testing.OnActionExecutionAsync(_actionExecutingContext, Next);
        }
    }
}
