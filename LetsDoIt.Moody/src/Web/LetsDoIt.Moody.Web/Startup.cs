using HealthChecks.UI.Client;
using LetsDoIt.Moody.Application.Client;
using LetsDoIt.Moody.Application.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;

namespace LetsDoIt.Moody.Web
{
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
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters();
                });

            services.AddResponseCompression();

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

            services.AddControllers();

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

            services.AddTransient<IEntityRepository<Category>, CategoryRepository>();
            services.AddTransient<IEntityRepository<VersionHistory>, VersionHistoryRepository>();
            services.AddTransient<IEntityRepository<Client>, ClientRepository>();
            services.AddTransient<IEntityRepository<CategoryDetails>, CategoryDetailsRepository>();

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IVersionHistoryService, VersionHistoryService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddSingleton<ISecurityService>(ss =>
                new SecurityService(JwtEncryptionKey, tokenExpirationMinutes));

            services.AddMvc(options =>
            {
                options.Filters.Add<TokenAuthorizationFilter>();
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

            app.UseResponseCompression();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

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
