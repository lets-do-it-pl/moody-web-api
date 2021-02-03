using System;

namespace LetsDoIt.Moody.Application.CustomExceptions
{
    public class MissingUserTypeException : Exception
    {
        public MissingUserTypeException()
            : base($"Wrong user type! Specified type does not exits or not allowed.")
        {
        }
    }
}
