using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace LetsDoIt.Moody.Web.Extensions
{
    using Filters;

    public static class AddMvcExtension
    {
        public static IServiceCollection AddActionFilterAndNewstonSoftJson(this IServiceCollection services)
        {
            services.AddMvc()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddMvcOptions(opt =>
                    opt.Filters.Add<LoggingActionFilter>());

            return services;
        }
    }
}
