using System.Linq;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LetsDoIt.Moody.Web.Filters
{
    public class TokenAuthorizationFilter:IAsyncActionFilter
    {
        private readonly IUserService userService;
        public TokenAuthorizationFilter(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
             var authorizationAttritbute = context.Filters.OfType<TokenAuthorizationAttritbute>().FirstOrDefault();

             if (authorizationAttritbute != null)
             {
                if (!context.HttpContext.Request.Headers.TryGetValue("Token", out var tokens))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                if (await userService.IsTokenValidAsync(tokens[0]) == false)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
             }

             await next();
        }
    }

}
