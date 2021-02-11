using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LetsDoIt.Moody.Web
{
    using Extensions;
    using Middleware;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthenticationConfig(Configuration)
                .AddOptionsConfig(Configuration)
                .AddAuthorizationConfig()
                .AddResponseCompression()
                .AddDbContextConfig(Configuration)
                .AddCorsConfig(Configuration)
                .AddSwaggerConfig()
                .AddMailSender()
                .AddCustomClasses()
                .AddActionFilterAndNewstonSoftJson()
                .AddHealthCheckConfig(Configuration)
                .AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Azure"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseApiExceptionHandler();
            }


            app.UseResponseCompression();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCorsConfig(Configuration);

            app.UseAuthentication()
                .UseAuthorization();

            app.UseCustomSwaggerConfig();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecksConfig();
            });
        }
    }
}