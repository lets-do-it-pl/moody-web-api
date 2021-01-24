using Castle.DynamicProxy;
using LetsDoIt.Moody.Application.Interceptors;
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
            .AddSingleton(new ProxyGenerator())
            .AddSingleton<IAsyncInterceptor, LoggingInterceptor>()  
            .AddTransient<IApplicationContext, ApplicationContext>()
            .AddProxiedTransient<ICategoryRepository, CategoryRepository>()
            .AddProxiedTransient<IRepository<VersionHistory>, VersionHistoryRepository>()
            .AddProxiedTransient<IRepository<Client>, ClientRepository>()
            .AddProxiedTransient<IRepository<User>, UserRepository>()
            .AddProxiedTransient<IRepository<CategoryDetail>, CategoryDetailsRepository>()

            .AddProxiedTransient<ICategoryService, CategoryService>()
            .AddProxiedTransient<IVersionHistoryService, VersionHistoryService>()
            .AddProxiedTransient<IClientService, ClientService>()
            .AddProxiedSingleton<ISecurityService, SecurityService>()
            .AddProxiedTransient<IUserService, UserService>()
            .AddProxiedTransient<IDashboardService, DashboardService>()
            .AddProxiedTransient<ISearchService, SearchService>()
            .AddProxiedTransient<IDataService , DataService>();
    }
}
