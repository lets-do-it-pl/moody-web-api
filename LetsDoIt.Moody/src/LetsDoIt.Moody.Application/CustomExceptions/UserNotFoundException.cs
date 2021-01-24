using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoIt.Moody.Application.CustomExceptions
{
    public class UserNotFoundException:Exception
    {
        public UserNotFoundException() 
            : base("User could not be found!")
        {
            
        }
    }
}
