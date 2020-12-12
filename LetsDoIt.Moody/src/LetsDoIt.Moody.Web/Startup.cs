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
            var webUrl = Configuration.GetValue<string>("WebUrl");

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

            services.AddCors(options =>
            {
                options.AddPolicy("AnotherPolicy",
                    builder => {
                        builder.WithOrigins(webUrl).AllowAnyOrigin();
                    });
            });
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

            app.UseCors(options => options.AllowAnyOrigin());

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
