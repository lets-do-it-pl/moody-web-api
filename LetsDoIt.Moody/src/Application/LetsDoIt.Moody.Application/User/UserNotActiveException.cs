using System;
using System.Runtime.Serialization;

namespace LetsDoIt.Moody.Application.User
{
    [Serializable]
    internal class UserNotActiveException : Exception
    {
        public UserNotActiveException()
        {
        }

        public UserNotActiveException(string message) : base(message)
        {
        }

        public UserNotActiveException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserNotActiveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}