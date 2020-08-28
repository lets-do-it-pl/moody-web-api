using System;
using Microsoft.AspNetCore.Builder;

namespace LetsDoIt.Moody.Web.Middleware
{
    public static class ApiExceptionMiddlewareExtensions
    {        
        public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiExceptionMiddleware>();
        }
    }
}
