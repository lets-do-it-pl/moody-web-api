using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoIt.Moody.Application.CustomExceptions
{
    public class UserNotFoundException:Exception
    {
        public UserNotFoundException(int id) 
            : base($"{id} numbered user could not be found!")
        {
            
        }
    }
}
