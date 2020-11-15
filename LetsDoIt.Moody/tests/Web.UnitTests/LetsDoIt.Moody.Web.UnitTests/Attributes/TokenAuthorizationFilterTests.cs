using System.Collections.Generic;
using LetsDoIt.Moody.Web.Filters;
using System.Threading.Tasks;
using FluentAssertions;
using LetsDoIt.Moody.Application.Client;
using LetsDoIt.Moody.Application.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Web.UnitTests.Attributes
{
    public class TokenAuthorizationFilterTests
    {
        private readonly TokenAuthorizationFilter _testing;
        private readonly Mock<ISecurityService> _securityService;
        private readonly ActionExecutingContext _actionExecutingContext;
        private readonly ActionExecutedContext _context;

        public TokenAuthorizationFilterTests()
        {
            var authorizationAttribute = new Mock<Authorization>();

            _securityService = new Mock<ISecurityService>();

            _testing = new TokenAuthorizationFilter(_securityService.Object);

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

            _securityService.Setup(us => us.ValidateToken(token)).Returns(false);

            Task<ActionExecutedContext> Next() => Task.FromResult(_context);

            await _testing.OnActionExecutionAsync(_actionExecutingContext, Next);

            _actionExecutingContext.Result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldInvokeUserServiceIsTokenValid()
        {
            var token = "Bearer good.token";

            _actionExecutingContext.HttpContext.Request.Headers.Add("Authorization", token);

            _securityService.Setup(us => us.ValidateToken(token)).Returns(false);

            Task<ActionExecutedContext> Next() => Task.FromResult(_context);

            await _testing.OnActionExecutionAsync(_actionExecutingContext, Next);

            _securityService.Verify(us => us.ValidateToken(It.IsAny<string>()), Times.Once);
        }
    }
}
