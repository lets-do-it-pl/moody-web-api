using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetsDoIt.Moody.Web.Extensions
{
    using Persistence;

    public static class HealthCheckConfigurationExtension
    {
        public static IServiceCollection AddHealthCheckConfig(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationContext>();

            services.AddHealthChecksUI(opt =>
                {
                    opt.SetEvaluationTimeInSeconds(300);
                    opt.MaximumHistoryEntriesPerEndpoint(60);
                    opt.SetApiMaxActiveRequests(1);
                    opt.SetMinimumSecondsBetweenFailureNotifications(1800);
                    opt.AddHealthCheckEndpoint("For Database", "/health");
                })
                .AddInMemoryStorage();

            return services;
        }

        public static IEndpointRouteBuilder MapHealthChecksConfig(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            //Default url is /healthchecks-ui
            endpoints.MapHealthChecksUI();

            return endpoints;
        }
    }
}
