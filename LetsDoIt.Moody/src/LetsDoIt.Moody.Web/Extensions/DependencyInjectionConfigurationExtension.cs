using Microsoft.Extensions.DependencyInjection;

namespace LetsDoIt.Moody.Web.Extensions
{
    using Application.Category;
    using Application.Client;
    using Application.Security;
    using Application.User;
    using Application.VersionHistory;
    using Persistence.Entities;
    using Persistence.Repositories;
    using Persistence.Repositories.Base;
    using Persistence.Repositories.Category;
    public static class DependencyInjectionConfigurationExtension
    {
        public static IServiceCollection AddCustomClasses(this IServiceCollection services)
        {
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IRepository<VersionHistory>, VersionHistoryRepository>();
            services.AddTransient<IRepository<Client>, ClientRepository>();
            services.AddTransient<IRepository<User>, UserRepository>();
            services.AddTransient<IRepository<CategoryDetail>, CategoryDetailsRepository>();

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IVersionHistoryService, VersionHistoryService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddSingleton<ISecurityService, SecurityService>();
            services.AddTransient<IUserService, UserService>();

            return services;
        }
    }
}
