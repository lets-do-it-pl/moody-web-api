﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LetsDoIt.Moody.Web
{
    using Infrastructure;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TemporaryTokenValidatorAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "Token";        

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var temporaryToken = TemporaryToken.ValidateTemporaryToken(token);

            if(!temporaryToken)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
    

