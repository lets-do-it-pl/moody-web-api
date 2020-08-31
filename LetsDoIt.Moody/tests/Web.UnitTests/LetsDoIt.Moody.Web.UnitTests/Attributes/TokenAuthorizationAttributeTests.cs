using System;
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

    public class TokenAuthorizationAttributeTests
    {
        private readonly TokenAuthorizationAttritbute _testing;
        private readonly ActionExecutingContext _actionExecutingContext;
        private readonly ActionExecutedContext _context;

        public TokenAuthorizationAttributeTests()
        {
            _testing = new TokenAuthorizationAttritbute();

             var defaultHttpContext = new DefaultHttpContext();

             var actionContext = new ActionContext(
                 defaultHttpContext,
                 Mock.Of<RouteData>(),
                 Mock.Of<ActionDescriptor>());

             _actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>()
            );

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
        public async Task OnActionExecutionAsync_WhenIsNotValid_ShouldReturnUnauthorizedResult()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var userService = new Mock<IUserService>();

            var token = "bad.token";

            userService.Setup(us => us.IsTokenValidAsync(token)).ReturnsAsync(false);

            serviceProvider
                .Setup(x => x.GetService(typeof(IUserService)))
                .Returns(userService.Object);

            Task<ActionExecutedContext> Next() => Task.FromResult(_context);

            await _testing.OnActionExecutionAsync(_actionExecutingContext, Next);

            _actionExecutingContext.Result.Should().BeOfType<UnauthorizedResult>();
        }

    }
}
