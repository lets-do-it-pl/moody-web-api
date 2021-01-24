using Microsoft.Extensions.DependencyInjection;

namespace LetsDoIt.Moody.Web.Extensions
{
    using Application.Category;
    using Application.Client;
    using Application.Dashboard;
    using Application.Data;
    using Persistence.Entities;
    using Persistence.Repositories;
    using Persistence.Repositories.Base;
    using Persistence.Repositories.Category;
    using Application.Search;
    using Application.Security;
    using Application.User;
    using Application.VersionHistory;
    using Persistence;
    

    public static class DependencyInjectionConfigurationExtension
    {
        public static IServiceCollection AddCustomClasses(this IServiceCollection services) => services
            .AddTransient<IApplicationContext, ApplicationContext>()
            .AddTransient<ICategoryRepository, CategoryRepository>()
            .AddTransient<IRepository<VersionHistory>, VersionHistoryRepository>()
            .AddTransient<IRepository<Client>, ClientRepository>()
            .AddTransient<IRepository<User>, UserRepository>()
            .AddTransient<IRepository<CategoryDetail>, CategoryDetailsRepository>()

            .AddTransient<ICategoryService, CategoryService>()
            .AddTransient<IVersionHistoryService, VersionHistoryService>()
            .AddTransient<IClientService, ClientService>()
            .AddSingleton<ISecurityService, SecurityService>()
            .AddTransient<IUserService, UserService>()
            .AddTransient<IDashboardService, DashboardService>()
            .AddTransient<ISearchService, SearchService>()
            .AddTransient<IDataService , DataService>();
    }
}
