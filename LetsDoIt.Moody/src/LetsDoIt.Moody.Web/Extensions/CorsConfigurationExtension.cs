using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace LetsDoIt.Moody.Web.Extensions
{
    using Application.Options;

    public static class CorsConfigurationExtension
    {
        public static IServiceCollection AddCorsConfig(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var url = GetWebUrl(configuration);

            return services
                .AddCors(options =>
                {
                    options.AddPolicy("AllowOrigin",
                        builder =>
                        {
                            builder
                            .WithOrigins(url);
                        });
                });
        }

        public static IApplicationBuilder UseCorsConfig(
            this IApplicationBuilder app, 
            IConfiguration configuration)
        {
            var url = GetWebUrl(configuration);

            app.UseCors(options => options.WithOrigins(url));
            
            return app;
        }

        private static string GetWebUrl(IConfiguration configuration) => 
            configuration.GetValue<string>($"{WebInfoOptions.WebInfoSectionName}:Url");
    }
}
