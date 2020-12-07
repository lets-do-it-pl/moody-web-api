using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LetsDoIt.Moody.Application.Interceptors
{
    public class LoggingInterceptor : IAsyncInterceptor
    {
        private readonly ILogger<LoggingInterceptor> _logger;

        public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
        {
            _logger = logger;
        }

        public void InterceptSynchronous(IInvocation invocation)
        {
            _logger.LogInformation($"Start method {invocation.TargetType}.{invocation.Method.Name} with parameters: {JsonConvert.SerializeObject(invocation.Arguments)}");

            invocation.Proceed();

            _logger.LogInformation($"Finish method {invocation.TargetType}.{invocation.Method.Name} with return value: {JsonConvert.SerializeObject(invocation.ReturnValue)}");
        }

        public void InterceptAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptAsynchronous(invocation);
        }

        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
        }

        private async Task InternalInterceptAsynchronous(IInvocation invocation)
        {
            _logger.LogInformation($"Start method {invocation.TargetType}.{invocation.Method.Name}" +
                                   $" with parameters: {(invocation.Method.GetParameters()[0].ParameterType.ToString().Contains("Linq") ? "Linq Expression" : JsonConvert.SerializeObject(invocation.Arguments))}");

            invocation.Proceed();

            var task = (Task)invocation.ReturnValue;

            await task;

            _logger.LogInformation($"Finish method {invocation.TargetType}.{invocation.Method.Name} for async Task.");

        }

        private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            _logger.LogInformation($"Start method {invocation.TargetType}.{invocation.Method.Name}" +
                                   $" with parameters: {(invocation.Method.GetParameters()[0].ParameterType.ToString().Contains("Linq") ? "Linq Expression" : JsonConvert.SerializeObject(invocation.Arguments))}");

            invocation.Proceed();

            var task = (Task<TResult>)invocation.ReturnValue;

            TResult result = await task;

            _logger.LogInformation($"Finish method {invocation.TargetType}.{invocation.Method.Name}" +
                                   $" with return value for async Task<T>: {JsonConvert.SerializeObject(result)}");

            return result;
        }
    }
}
