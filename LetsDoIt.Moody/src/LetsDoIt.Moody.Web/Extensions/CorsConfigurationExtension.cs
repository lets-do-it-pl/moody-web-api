using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetsDoIt.Moody.Web.Extensions
{
    public static class CorsConfigurationExtension
    {

        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            var webUrl = configuration.GetValue<string>("WebUrl");

            return services
                .AddCors(options =>
            {
                options.AddPolicy("AnotherPolicy",
                    builder =>
                    {
                        builder
                        .WithOrigins(webUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                    });
            });
        }
    }
}
