using System;

namespace LetsDoIt.Moody.Application.CustomExceptions
{
    public class EmailNotRegisteredException:Exception
    {
        public EmailNotRegisteredException(string email)
            : base($"{email} : email is not registered!")
        {
        }
    }
}
