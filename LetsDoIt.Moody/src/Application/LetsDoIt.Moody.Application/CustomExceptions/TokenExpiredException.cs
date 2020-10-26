using System;

namespace LetsDoIt.Moody.Application.CustomExceptions
{
    public class TokenExpiredException:Exception
    {
        public TokenExpiredException(string name)
            : base($"Token for {name} has expired!")
        {
        }
    }
}
