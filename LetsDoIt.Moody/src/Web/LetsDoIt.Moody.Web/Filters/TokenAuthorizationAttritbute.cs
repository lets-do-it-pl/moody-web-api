using System;
using System.Security;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace LetsDoIt.Moody.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TokenAuthorizationAttritbute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("Token", out var tokens))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

            try
            {
                await userService.ValidateTokenAsync(tokens[0]);

            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException || ex is SecurityException) 
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            catch (Exception)
            {
                throw;
            }

            await next();
        }
    }
}
