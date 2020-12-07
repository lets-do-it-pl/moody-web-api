using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetsDoIt.Moody.Web.Extensions
{
    using Persistence;

    public static class DbContextConfigurationExtension
    {
        public static IServiceCollection AddDbContextConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MoodyDBConnection");

            services.AddDbContext<ApplicationContext>(opt =>
                opt
                    .UseLazyLoadingProxies()
                    .UseSqlServer(connectionString));

            return services;
        }
    }
}
