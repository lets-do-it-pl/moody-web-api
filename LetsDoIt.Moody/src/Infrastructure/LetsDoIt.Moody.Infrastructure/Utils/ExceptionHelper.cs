using System;

namespace LetsDoIt.Moody.Infrastructure
{
    public static class ExceptionHelper
    {
        public static string GetExceptionMessages(this Exception exception)
        {
            return exception.InnerException != null ? GetExceptionMessages(exception.InnerException) : exception.Message;
        }
    }
}
