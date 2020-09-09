﻿using System.Linq;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LetsDoIt.Moody.Web.Filters
{
    public class TokenAuthorizationFilter:IAsyncActionFilter
    {
        private readonly IUserService _userService;
        private const string Token = "Token";
        public TokenAuthorizationFilter(IUserService userService)
        {
            _userService = userService;
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

                var isValidToken = await _userService.ValidateTokenAsync(tokens[0]);
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
