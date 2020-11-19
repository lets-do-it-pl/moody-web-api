using System.Collections.Generic;
using LetsDoIt.Moody.Web.Filters;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Web.UnitTests.Attributes
{
    using Application.User;

    public class TokenAuthorizationFilterTests
    {
        private readonly TokenAuthorizationFilter _testing;
        private readonly Mock<IUserService> _userService;
        private readonly ActionExecutingContext _actionExecutingContext;
        private readonly ActionExecutedContext _context;

        public TokenAuthorizationFilterTests()
        {
            var authorizationAttribute = new Mock<Authorization>();

            _userService = new Mock<IUserService>();

            _testing = new TokenAuthorizationFilter(_userService.Object);

             var defaultHttpContext = new DefaultHttpContext();

             var actionContext = new ActionContext(
                 defaultHttpContext,
                 Mock.Of<RouteData>(),
                 Mock.Of<ActionDescriptor>());

             _actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>
                {
                    authorizationAttribute.Object
                },
                new Dictionary<string, object>(),
                Mock.Of<Controller>());

              _context = new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), Mock.Of<Controller>());
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenTokenHeaderIsMissing_ShouldReturnUnauthorizedResult()
        {
            Task<ActionExecutedContext> Next() => Task.FromResult(_context);

            await _testing.OnActionExecutionAsync(_actionExecutingContext, Next);

            _actionExecutingContext.Result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenTokenIsNotValid_ShouldReturnUnauthorizedResult()
        {
            var token = "Bearer bad.token";

            _actionExecutingContext.HttpContext.Request.Headers.Add("Authorization", token);

            _userService.Setup(us => us.ValidateTokenAsync(token)).ReturnsAsync(false);

            Task<ActionExecutedContext> Next() => Task.FromResult(_context);

            await _testing.OnActionExecutionAsync(_actionExecutingContext, Next);

            _actionExecutingContext.Result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldInvokeUserServiceIsTokenValid()
        {
            var token = "Bearer good.token";

            _actionExecutingContext.HttpContext.Request.Headers.Add("Authorization", token);

            _userService.Setup(us => us.ValidateTokenAsync(token)).ReturnsAsync(false);

            Task<ActionExecutedContext> Next() => Task.FromResult(_context);

            await _testing.OnActionExecutionAsync(_actionExecutingContext, Next);

            _userService.Verify(us=>us.ValidateTokenAsync(It.IsAny<string>()),Times.Once);
        }
    }
}
