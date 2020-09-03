using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LetsDoIt.Moody.Web
{
    using Infrastructure;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TemporaryTokenValidatorAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "Validate";
        private readonly ITemporaryToken token;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var guid))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var temporaryToken = token.TemporaryTokenValidator(guid);

            if(temporaryToken == false)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
    

