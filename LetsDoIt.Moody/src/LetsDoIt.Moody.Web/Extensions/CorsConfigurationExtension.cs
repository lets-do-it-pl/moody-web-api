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
                    options.AddPolicy("AdminDashboardPolicy",
                        builder =>
                        {
                            builder
                                .WithOrigins(url)
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        });
                        options.AddPolicy("MobilePolicy", builder =>
                        {
                            builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        });
                });
        }

        public static IApplicationBuilder UseCorsConfig(
            this IApplicationBuilder app, 
            IConfiguration configuration)
        {

            app.UseCors("AdminDashboardPolicy");
            
            return app;
        }

        private static string GetWebUrl(IConfiguration configuration) => 
            configuration.GetValue<string>($"{WebInfoOptions.WebInfoSectionName}:Url");
    }
}
