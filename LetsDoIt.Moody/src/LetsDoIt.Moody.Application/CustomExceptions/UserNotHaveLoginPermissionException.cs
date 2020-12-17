using System;

namespace LetsDoIt.Moody.Application.CustomExceptions
{
    public class UserNotHaveLoginPermissionException : Exception
    {
        public UserNotHaveLoginPermissionException(string username)
            : base($"{username} : can not login the system. Please wait for admin to approve your login request!")
        {
        }
    }
}

