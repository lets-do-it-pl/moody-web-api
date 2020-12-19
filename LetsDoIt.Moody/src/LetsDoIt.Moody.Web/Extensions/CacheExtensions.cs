using LetsDoIt.Moody.Application.Client;
using LetsDoIt.Moody.Web.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetsDoIt.Moody.Web.Extensions
{
    public class CacheExtensions
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
            {
            var redisCacheSettings = new RedisCacheSettings();
            configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);
            services.AddSingleton(redisCacheSettings);

            if(!redisCacheSettings.Enable)
            {
                return;
            }
            services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            }
    }
}
