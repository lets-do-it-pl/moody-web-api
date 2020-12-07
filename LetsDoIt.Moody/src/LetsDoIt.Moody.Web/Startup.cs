using LetsDoIt.MailSender.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LetsDoIt.Moody.Web
{
    using Application.Options;
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
            services.Configure<JwtOptions>(Configuration.GetSection(JwtOptions.Jwt));

            services.AddAuthenticationConfig(Configuration);

            services.AddAuthorizationConfig();

            services.AddResponseCompression();

            services.AddHealthCheckConfig(Configuration);

            services.AddDbContextConfig(Configuration);

            services.AddControllers();

            services.AddSwaggerConfig();

            services.Configure<SmtpOptions>(Configuration.GetSection(SmtpOptions.SmtpSectionName));

            services.AddMailSender();

            services.AddCustomClasses();
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCustomSwaggerConfig();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecksConfig();
            });
        }
    }
}
