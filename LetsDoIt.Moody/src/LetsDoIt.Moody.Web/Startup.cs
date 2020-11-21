using System;
using System.Text;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
<<<<<<< HEAD:LetsDoIt.Moody/src/Web/LetsDoIt.Moody.Web/Startup.cs
=======
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
>>>>>>> master:LetsDoIt.Moody/src/LetsDoIt.Moody.Web/Startup.cs

namespace LetsDoIt.Moody.Web
{
    using Application.Category;
<<<<<<< HEAD:LetsDoIt.Moody/src/Web/LetsDoIt.Moody.Web/Startup.cs
    using Application.User;
=======
    using Application.Client;
    using Application.Constants;
    using Application.Options;
    using Application.Security;
>>>>>>> master:LetsDoIt.Moody/src/LetsDoIt.Moody.Web/Startup.cs
    using Application.VersionHistory;
    using Entities;
    using Middleware;
    using Persistence;
    using Persistence.Entities;
    using Persistence.Repositories;
    using Persistence.Repositories.Base;
    using Persistence.Repositories.Category;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JwtOptions>(Configuration.GetSection(JwtOptions.Jwt));

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
                        ClockSkew = TimeSpan.Zero,
                        RequireExpirationTime = true
                    };
                });

            services.AddAuthorization(config =>
            {
                config.AddPolicy(UserTypeConstants.Admin, Policies.AdminPolicy());
                config.AddPolicy(UserTypeConstants.Standard, Policies.StandardPolicy());
                config.AddPolicy(UserTypeConstants.Client, Policies.ClientPolicy());
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
                opt
                    .UseLazyLoadingProxies()
                    .UseSqlServer(connectionString));

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

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {securitySchema, new[] {"Bearer"}}
                };
                c.AddSecurityRequirement(securityRequirement);
            });

<<<<<<< HEAD:LetsDoIt.Moody/src/Web/LetsDoIt.Moody.Web/Startup.cs
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
=======
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IRepository<VersionHistory>, VersionHistoryRepository>();
            services.AddTransient<IRepository<Client>, ClientRepository>();
            services.AddTransient<IRepository<User>, UserRepository>();
            services.AddTransient<IRepository<CategoryDetail>, CategoryDetailsRepository>();

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IVersionHistoryService, VersionHistoryService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddSingleton<ISecurityService, SecurityService>();
>>>>>>> master:LetsDoIt.Moody/src/LetsDoIt.Moody.Web/Startup.cs
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

            app.UseResponseCompression();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

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
