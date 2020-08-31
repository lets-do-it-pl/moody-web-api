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
    public class TokenAuthorizationAttributeTests
    {
        private readonly TokenAuthorizationAttritbute _testing;
       


        public TokenAuthorizationAttributeTests()
        {
            _testing = new TokenAuthorizationAttritbute();
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenTokenHeaderIsMissing_ShouldReturnUnauthorizedResult()
        {

            var defaultHttpContext = new DefaultHttpContext();

            var actionContext = new ActionContext(
                defaultHttpContext,
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>());

            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>()
            );

            var context = new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), Mock.Of<Controller>());

            Task<ActionExecutedContext> Next() => Task.FromResult(context);

            await _testing.OnActionExecutionAsync(actionExecutingContext, Next);

            actionExecutingContext.Result.Should().BeOfType<UnauthorizedResult>();
        }

    }
}
