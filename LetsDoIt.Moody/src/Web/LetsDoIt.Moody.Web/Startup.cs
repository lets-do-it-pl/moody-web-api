using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace LetsDoIt.Moody.Web
{
    using Application.Category;
    using Application.User;
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

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

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

            var webUrl = Configuration.GetValue<string>("WebUrl");

            services.AddCors(options =>
            {
                options.AddPolicy("AnotherPolicy",
                    builder =>{
                        builder.WithOrigins(webUrl).AllowAnyHeader().AllowAnyMethod();
                    });
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
            app.UseCors();

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
