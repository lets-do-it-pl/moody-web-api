using System;

namespace LetsDoIt.Moody.Application.CustomExceptions
{
    public class UserNotHaveLoginPermissionException : Exception
    {
        public UserNotHaveLoginPermissionException()
            : base($"User can not login the system. Please wait for the admin to approve your login request!")
        {
        }
    }
}

