using LetsDoIt.Moody.Application.User;
using LetsDoIt.Moody.Application.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace LetsDoIt.Moody.Web
{
    using Persistance;
    using Persistance.Repositories.Base;
    using Application.Category;
    using Application.VersionHistory;
    using LetsDoIt.Moody.Persistance.Repositories;
    using LetsDoIt.Moody.Domain;

    public class Startup
    {
        private IConfiguration _config;
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression();

            services.AddDbContext<ApplicationContext>(opt =>
              opt.UseSqlServer(_config.GetConnectionString("MoodyDBConnection"),
                  x => x.MigrationsAssembly("LetsDoIt.Moody.Web")));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Moody API",
                    Version = "v1",
                    Description = "Moody API details are here."
                });
            });

            services.AddTransient<ProtectionHelper>();
            
            services.AddTransient<IEntityRepository<Category>, CategoryRepository>();
            services.AddTransient<IEntityRepository<VersionHistory>, VersionHistoryRepository>();
            services.AddTransient<IEntityRepository<User>, UserRepository>();

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IVersionHistoryService, VersionHistoryService>();
            services.AddTransient<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
            });
        }
    }
}
