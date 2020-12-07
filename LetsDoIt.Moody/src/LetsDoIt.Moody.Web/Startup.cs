using LetsDoIt.MailSender.Options;
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
            services.AddAuthenticationConfig(Configuration)
                .AddAuthorizationConfig()
                .AddResponseCompression()
                .AddHealthCheckConfig(Configuration)
                .AddDbContextConfig(Configuration)
                .AddSwaggerConfig()
                .AddMailSender()
                .AddCustomClasses();

            services.AddControllers();

            services.Configure<SmtpOptions>(Configuration.GetSection(SmtpOptions.SmtpSectionName));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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
