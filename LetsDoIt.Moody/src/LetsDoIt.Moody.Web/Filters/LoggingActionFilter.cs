using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LetsDoIt.Moody.Web.Filters
{
    public class LoggingActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation($"Start Action '{context.ActionDescriptor.DisplayName}' with {JsonConvert.SerializeObject(context.ActionArguments.Values)}");

            var resultContext = await next();

            if (resultContext.Result != null)
            {
                var resultType = resultContext.Result.GetType();

                _logger.LogInformation($"Finish Action '{context.ActionDescriptor.DisplayName}' with result: {resultType.Name}");
            }
        }
    }
}
