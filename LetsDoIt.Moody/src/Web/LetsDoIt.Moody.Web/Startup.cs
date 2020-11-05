using System.Reflection;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using LetsDoIt.MailSender;
using LetsDoIt.MailSender.Options;  
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.ResponseCompression;

namespace LetsDoIt.Moody.Web
{
    using Application.User;
    using Application.Category;
    using Application.VersionHistory;
    using Domain;
    using Filters;
    using Middleware;
    using Persistance;
    using Persistance.Repositories;
    using Persistance.Repositories.Base;

    public class Startup
    {
        private const string JwtEncryptionKey = "2hN70OoacUi5SDU0rNuIXg==";
        private const string InMemoryProviderName = "Microsoft.EntityFrameworkCore.InMemory";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var connectionString = Configuration.GetConnectionString("MoodyDBConnection");

            services
                .AddHealthChecks()
                .AddSqlServer(connectionString, "SELECT 1", name: "SqlServerApplicationDb");

            var url = Configuration.GetValue<string>("HealthChecksUri");

            services.AddHealthChecksUI(s =>
            {
                s.AddHealthCheckEndpoint("Moody API", url);
            })
            .AddInMemoryStorage();

            services.AddDbContext<ApplicationContext>(opt =>
                opt.UseLazyLoadingProxies()
                .UseSqlServer(
                    connectionString,
                    builder =>
                    {
                        builder.MigrationsAssembly("LetsDoIt.Moody.Persistance");
                    }));

            services.AddControllers()
                .AddFluentValidation(opt =>
            {
                opt.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            }); ;

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

                var securityRequirement = new OpenApiSecurityRequirement();
                securityRequirement.Add(securitySchema, new[] { "Bearer" });
                c.AddSecurityRequirement(securityRequirement);
            });

            var tokenExpirationMinutes = Configuration.GetValue<int>("TokenExpirationMinutes");
            var emailVerificationTokenMinutes = Configuration.GetValue<int>("EmailVerificationTokenExpirationMinutes");

            services.Configure<SmtpOptions>(Configuration.GetSection(SmtpOptions.SmtpSectionName));

            services.AddMailSender();

            services.AddTransient<IEntityRepository<Category>, CategoryRepository>();
            services.AddTransient<IEntityRepository<VersionHistory>, VersionHistoryRepository>();
            services.AddTransient<IEntityRepository<User>, UserRepository>();
            services.AddTransient<IEntityRepository<UserToken>, UserTokenRepository>();
            services.AddTransient<IEntityRepository<CategoryDetails>, CategoryDetailsRepository>();
            services.AddTransient<IEntityRepository<EmailVerificaitonToken>, EmailVerificationTokenRepository>();

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IVersionHistoryService, VersionHistoryService>();
            services.AddTransient<IUserService>(us => new UserService(
                    us.GetService<IEntityRepository<User>>(),
                    us.GetService<IEntityRepository<UserToken>>(),
                    JwtEncryptionKey,
                    tokenExpirationMinutes,
                    emailVerificationTokenMinutes,
                    us.GetService<IMailSender>(),
                    us.GetService<IEntityRepository<EmailVerificaitonToken>>()
                ));

            services.AddMvc(options =>
            {
                options.Filters.Add<TokenAuthorizationFilter>();
            });

            services.AddResponseCompression(options =>
                {
                    options.EnableForHttps = true;
                    options.Providers.Add<GzipCompressionProvider>();
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env,
            ApplicationContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseApiExceptionHandler();
            }
            
            if (context.Database.ProviderName != InMemoryProviderName)
            {
                context.Database.Migrate();
            }


            app.UseHttpsRedirection();

            app.UseResponseCompression();

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Moody API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecksUI();

                endpoints.MapHealthChecks("/healthCheck", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });


        }

    }
}
