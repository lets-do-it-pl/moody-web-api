using System;

namespace LetsDoIt.Moody.Application.Utils
{
    public static class ExceptionHelper
    {
        public static string GetInnermostExceptionMessage(this Exception exception)
        {
            return exception.InnerException != null ? GetInnermostExceptionMessage(exception.InnerException) : exception.Message;
        }
    }
}
