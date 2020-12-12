using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetsDoIt.Moody.Web.Extensions
{
    public static class HealthCheckConfigurationExtension
    {
        public static IServiceCollection AddHealthCheckConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MoodyDBConnection");

            services
                .AddHealthChecks()
                .AddSqlServer(connectionString, "SELECT 1", name: "SqlServerApplicationDb");

            var url = configuration.GetValue<string>("HealthChecksUri");

            services.AddHealthChecksUI(s =>
                {
                    s.AddHealthCheckEndpoint("Moody API", url);
                })
                .AddInMemoryStorage();

            return services;
        }
    }
}
