using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetsDoIt.Moody.Web.Extensions
{
    using Application.Options;

    public static class CorsConfigurationExtension
    {
        public static IServiceCollection AddCorsConfig(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var url = configuration.GetValue<string>($"{WebInfoOptions.WebInfoSectionName}:Url");

            return services
                .AddCors(options =>
            {
                options.AddPolicy("AnotherPolicy",
                    builder =>
                    {
                        builder
                        .WithOrigins(url)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                    });
            });
        }
    }
}
