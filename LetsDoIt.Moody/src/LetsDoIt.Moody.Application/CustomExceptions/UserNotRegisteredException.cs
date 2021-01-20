using System;

namespace LetsDoIt.Moody.Application.CustomExceptions
{
    public class UserNotRegisteredException:Exception
    {
        public UserNotRegisteredException(string email)
            : base($"{email} : user is not registered!")
        {
        }
    }
}
