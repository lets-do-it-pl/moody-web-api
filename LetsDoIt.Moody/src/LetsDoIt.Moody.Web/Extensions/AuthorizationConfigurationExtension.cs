using Microsoft.Extensions.DependencyInjection;

namespace LetsDoIt.Moody.Web.Extensions
{
    using Application.Constants;
    using Entities;
    public static class AuthorizationConfigurationExtension
    {
        public static IServiceCollection AddAuthorizationConfig(this IServiceCollection services)
        {
            services.AddAuthorization(config =>
            {
                config.AddPolicy(UserTypeConstants.Admin, Policies.AdminPolicy());
                config.AddPolicy(UserTypeConstants.Standard, Policies.StandardPolicy());
                config.AddPolicy(UserTypeConstants.Client, Policies.ClientPolicy());
            });

            return services;
        }
    }
}



