using System;

namespace LetsDoIt.Moody.Application.CustomExceptions
{
    public class UserNotActiveException :Exception
    {
        public UserNotActiveException()
            : base($"User is not activated! Please check your mailbox!")
        {
        }
    }
}
