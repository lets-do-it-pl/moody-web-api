using System;

namespace LetsDoIt.Moody.Application.CustomExceptions
{
    public class UserNotActiveException :Exception
    {
        public UserNotActiveException(string username)
            : base($"{username} : user is not activated! Please check you mailbox out!")
        {
        }
    }
}
