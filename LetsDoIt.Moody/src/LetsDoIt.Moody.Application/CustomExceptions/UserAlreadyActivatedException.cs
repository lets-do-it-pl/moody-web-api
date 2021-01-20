using System;

namespace LetsDoIt.Moody.Application.CustomExceptions
{
    public class UserAlreadyActivatedException : Exception
    {
        public UserAlreadyActivatedException(string message = null)
            : base($"User is already activated! {message}")
        {
        }
    }
}
