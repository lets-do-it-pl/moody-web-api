using LetsDoIt.Moody.Web.Filters;
using LetsDoIt.Moody.Web.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;


namespace LetsDoIt.Moody.Web
{
    using Application.User;
    using Persistance;
    using Persistance.Repositories.Base;
    using Application.Category;
    using Application.VersionHistory;
    using Persistance.Repositories;
    using Domain;
    using HealthChecks.UI.Client;
    using HealthChecks.UI.Configuration;
    using System.Net;

    public class Startup
    {
        private const string JwtEncryptionKey = "2hN70OoacUi5SDU0rNuIXg==";
        private readonly IConfiguration _config;

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddResponseCompression();

            var connectionString = _config.GetConnectionString("MoodyDBConnection");
            services.AddDbContext<ApplicationContext>(opt => opt.UseSqlServer(connectionString));

            services
                .AddHealthChecks()
                .AddSqlServer(connectionString, "SELECT 1");

            var url = _config.GetValue<string>("HealthChecksUI:HealthChecks:Uri"); 

            services.AddHealthChecksUI(s=>
            {
                s.AddHealthCheckEndpoint("endpoint1", "https://localhost:1234/healthcheck");
            })
            .AddInMemoryStorage();

            services.AddDbContext<ApplicationContext>(opt =>
                opt.UseSqlServer(
                    connectionString,
                    builder =>
                    {
                        builder.MigrationsAssembly("LetsDoIt.Moody.Persistance");
                    }).UseLazyLoadingProxies());

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

            var tokenExpirationMinutes = _config.GetValue<int>("TokenExpirationMinutes");

            services.AddTransient<IEntityRepository<Category>, CategoryRepository>();
            services.AddTransient<IEntityRepository<VersionHistory>, VersionHistoryRepository>();
            services.AddTransient<IEntityRepository<User>, UserRepository>();
            services.AddTransient<IEntityRepository<UserToken>, UserTokenRepository>();
            services.AddTransient<IEntityRepository<CategoryDetails>, CategoryDetailsRepository>();

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IVersionHistoryService, VersionHistoryService>();
            services.AddTransient<IUserService>(us => new UserService(
                    us.GetService<IEntityRepository<User>>(),
                    us.GetService<IEntityRepository<UserToken>>(),
                    JwtEncryptionKey,
                    tokenExpirationMinutes
                ));

            services.AddMvc(options =>
            {
                options.Filters.Add<TokenAuthorizationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
