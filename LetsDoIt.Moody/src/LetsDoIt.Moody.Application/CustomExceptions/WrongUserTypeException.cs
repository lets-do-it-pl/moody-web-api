using System;

namespace LetsDoIt.Moody.Application.CustomExceptions
{
    public class WrongUserTypeException : Exception
    {
        public WrongUserTypeException()
            : base($"Wrong user type! Specified type does not exits or not allowed.")
        {
        }
    }
}
