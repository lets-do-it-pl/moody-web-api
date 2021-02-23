using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Interceptors
{
    using Infrastructure.Utils;

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

            if (invocation.Arguments == null || invocation.Arguments.Length == 0)
            {
                _logger.LogInformation($"Start method {invocation.TargetType}.{invocation.Method.Name}");
            }
            else if (invocation.Arguments[0].GetType().ToString().Contains("Linq"))
            {
                var formattedExpression = Evaluator.PartialEval((Expression)invocation.Arguments[0]);

                _logger.LogInformation($"Start method {invocation.TargetType}.{invocation.Method.Name}" +
                                       $" with expression parameter: {formattedExpression.Print()}");
            }
            else
            {
                _logger.LogInformation($"Start method {invocation.TargetType}.{invocation.Method.Name}" +
                                       $" with parameters: {(JsonConvert.SerializeObject(invocation.Arguments))}");
            }

            invocation.Proceed();

            var task = (Task)invocation.ReturnValue;

            await task;

            _logger.LogInformation($"Finish method {invocation.TargetType}.{invocation.Method.Name} for async Task.");
        }

        private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            if (invocation.Arguments == null || invocation.Arguments.Length == 0)
            {
                _logger.LogInformation($"Start method {invocation.TargetType}.{invocation.Method.Name}");
            }
            else if (invocation.Arguments[0].GetType().ToString().Contains("Linq"))
            {
                var formattedExpression = Evaluator.PartialEval((Expression)invocation.Arguments[0]);

                _logger.LogInformation($"Start method {invocation.TargetType}.{invocation.Method.Name}" +
                                       $" with expression parameter: {formattedExpression.Print()}");
            }
            else
            {
                _logger.LogInformation($"Start method {invocation.TargetType}.{invocation.Method.Name}" +
                                       $" with parameters: {(JsonConvert.SerializeObject(invocation.Arguments))}");
            }

            invocation.Proceed();

            var task = (Task<TResult>)invocation.ReturnValue;

            TResult result = await task;

            _logger.LogInformation($"Finish method {invocation.TargetType}.{invocation.Method.Name}" +
                                   $" with return value for async Task<T>: {JsonConvert.SerializeObject(result)}");

            return result;
        }
    }
}
