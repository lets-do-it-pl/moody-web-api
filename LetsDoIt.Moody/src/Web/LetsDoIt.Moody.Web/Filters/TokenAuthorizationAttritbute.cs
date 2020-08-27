using System;
using System.Linq;
using System.Threading.Tasks;
using LetsDoIt.Moody.Domain;
using LetsDoIt.Moody.Persistance.Repositories.Base;
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

            var userTokenRepository = context.HttpContext.RequestServices.GetRequiredService<IEntityRepository<UserToken>>();

            Console.WriteLine(tokens[0]);

            var userTokenDb = await userTokenRepository.GetAsync(ut => ut.Token == tokens[0] && ut.ExpirationDate > DateTime.UtcNow);

            if (userTokenDb==null)
            {
                context.Result=new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
