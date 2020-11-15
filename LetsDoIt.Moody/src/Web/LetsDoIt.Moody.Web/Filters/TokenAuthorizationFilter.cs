using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LetsDoIt.Moody.Web.Filters
{
    using Application.Security;

    public class TokenAuthorizationFilter : IAsyncActionFilter
    {
        private const string Token = "Authorization";

        private readonly ISecurityService _securityService;

        public TokenAuthorizationFilter(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authorizationAttritbute = context.Filters.OfType<Authorization>().FirstOrDefault();

            if (authorizationAttritbute != null)
            {
                if (!context.HttpContext.Request.Headers.TryGetValue(Token, out var tokens))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                var isValidToken = _securityService.ValidateToken(tokens[0]);
                if (!isValidToken)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }

            await next();
        }
    }

}
