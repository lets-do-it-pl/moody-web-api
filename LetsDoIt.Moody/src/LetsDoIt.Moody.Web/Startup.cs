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
                .AddCorsConfiguration(Configuration)
                .AddSwaggerConfig()
                .AddMailSender()
                .AddCustomClasses()
                .Configure<SmtpOptions>(Configuration.GetSection(SmtpOptions.SmtpSectionName));

            services.AddControllers();
<<<<<<< HEAD

            services.AddCors(options =>
            {
                options.AddPolicy("AnotherPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Moody API",
                    Version = "v1",
                    Description = "Moody API details are here."
                });

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {securitySchema, new[] {"Bearer"}}
                };
                c.AddSecurityRequirement(securityRequirement);
            });

            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IRepository<VersionHistory>, VersionHistoryRepository>();
            services.AddTransient<IRepository<Client>, ClientRepository>();
            services.AddTransient<IRepository<User>, UserRepository>();
            services.AddTransient<IRepository<CategoryDetail>, CategoryDetailsRepository>();

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IVersionHistoryService, VersionHistoryService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddSingleton<ISecurityService, SecurityService>();
=======
>>>>>>> master
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

            app.UseCors(options => options
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin());

            app.UseAuthentication()
                .UseAuthorization();

<<<<<<< HEAD
            app.UseCors();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Moody API V1");
            });
=======
            app.UseCustomSwaggerConfig();
>>>>>>> master

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecksConfig();
            });
        }
    }
}
